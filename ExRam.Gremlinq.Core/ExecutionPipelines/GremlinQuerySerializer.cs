using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Gremlin.Net.Process.Traversal;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct GroovySerializedGremlinQuery
    {
        public GroovySerializedGremlinQuery(string queryString, Dictionary<string, object> bindings)
        {
            QueryString = queryString;
            Bindings = bindings;
        }

        public override string ToString()
        {
            return QueryString;
        }

        public string QueryString { get; }
        public Dictionary<string, object> Bindings { get; }
    }

    public static class GremlinQuerySerializer
    {
        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            private readonly IImmutableDictionary<Type, AtomSerializer<object>> _dict;
            private readonly Lazy<ConcurrentDictionary<Type, AtomSerializer<object>?>> _lazyFastDict;

            public GremlinQuerySerializerImpl(IImmutableDictionary<Type, AtomSerializer<object>> dict)
            {
                _dict = dict;
                _lazyFastDict = new Lazy<ConcurrentDictionary<Type, AtomSerializer<object>?>>(
                    () => new ConcurrentDictionary<Type, AtomSerializer<object>?>(dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),
                    LazyThreadSafetyMode.PublicationOnly);
            }

            public object Serialize(IGremlinQuery query)
            {
                var _bindings = new Dictionary<object, Binding>();
                var _stepLabelNames = new Dictionary<StepLabel, string>();

                object Constant<TAtom>(TAtom atom, Func<TAtom, object> baseSerializer, Func<object, object> recurse)
                {
                    if (atom is StepLabel stepLabel)
                    {
                        if (!_stepLabelNames.TryGetValue(stepLabel, out var stepLabelMapping))
                        {
                            stepLabelMapping = "l" + (_stepLabelNames.Count + 1);
                            _stepLabelNames.Add(stepLabel, stepLabelMapping);
                        }

                        // ReSharper disable once TailRecursiveCall
                        return recurse(stepLabelMapping);
                    }

                    if (atom is IOptional optional)
                        return optional.MatchUntyped(recurse, () => Pick.None);

                    if (_bindings.TryGetValue(atom, out var binding))
                        return binding;

                    var bindingKey = string.Empty;
                    var next = _bindings.Count;

                    do
                    {
                        bindingKey = (char)('a' + next % 26) + bindingKey;
                        next = next / 26;
                    }
                    while (next > 0);

                    bindingKey = "_" + bindingKey;
                    binding = new Binding(bindingKey, atom);

                    _bindings.Add(atom, binding);

                    return binding;
                };

                object RecurseImpl(object o)
                {
                    var action = GetSerializer(o.GetType()) ?? Constant;

                    return action(o, _ => throw new NotImplementedException(), RecurseImpl);
                }

                return RecurseImpl(query);
            }

            public IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer)
            {
                return new GremlinQuerySerializerImpl(
                    _dict
                        .TryGetValue(typeof(TAtom))
                        .Match(
                            existingAtomSerializer => _dict.SetItem(typeof(TAtom), (atom, baseSerializer, recurse) => atomSerializer((TAtom)atom, _ => existingAtomSerializer(_, baseSerializer, recurse), recurse)),
                            () => _dict.SetItem(typeof(TAtom), (atom, baseSerializer, recurse) => atomSerializer((TAtom)atom, _ => throw new NotImplementedException(), recurse))));
            }

            private AtomSerializer<object>? GetSerializer(Type type)
            {
                return _lazyFastDict.Value
                    .GetOrAdd(
                        type,
                        closureType =>
                        {
                            foreach (var implementedInterface in closureType.GetInterfaces())
                            {
                                if (GetSerializer(implementedInterface) is AtomSerializer<object> interfaceSerializer)
                                    return interfaceSerializer;
                            }

                            if (closureType.BaseType is Type baseType)
                            {
                                if (GetSerializer(baseType) is AtomSerializer<object> baseSerializer)
                                    return baseSerializer;
                            }

                            return null;
                        });
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer)
            {
                throw new InvalidOperationException($"{nameof(OverrideAtomSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }

            public object Serialize(IGremlinQuery query)
            {
                throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        private sealed class SelectGremlinQuerySerializer : IGremlinQuerySerializer
        {
            private readonly Func<object, object> _projection;
            private readonly IGremlinQuerySerializer _baseSerializer;

            public SelectGremlinQuerySerializer(IGremlinQuerySerializer baseSerializer, Func<object, object> projection)
            {
                _baseSerializer = baseSerializer;
                _projection = projection;
            }

            public IGremlinQuerySerializer OverrideAtomSerializer<TAtom>(AtomSerializer<TAtom> atomSerializer)
            {
                return new SelectGremlinQuerySerializer(_baseSerializer.OverrideAtomSerializer(atomSerializer), _projection);
            }

            public object Serialize(IGremlinQuery query)
            {
                return _projection(_baseSerializer.Serialize(query));
            }
        }

        public static readonly GremlinqOption<bool> WorkaroundTinkerpop2112 = new GremlinqOption<bool>(false);

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Unit = new GremlinQuerySerializerImpl(ImmutableDictionary<Type, AtomSerializer<object>>.Empty);

        public static readonly IGremlinQuerySerializer Default = Unit
            .UseDefaultGremlinStepSerializationHandlers();

        private static readonly Step NoneWorkaround = new NotStep(GremlinQuery.Anonymous(GremlinQueryEnvironment.Default).Identity());

        public static IGremlinQuerySerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .OverrideAtomSerializer<IGremlinQuery>((query, overridden, recurse) =>
                {
                    var steps = query.AsAdmin().Steps.HandleAnonymousQueries();
                    if (query.AsAdmin().Environment.Options.GetValue(WorkaroundTinkerpop2112))
                        steps = steps.WorkaroundTINKERPOP_2112();

                    var byteCode = new Bytecode();

                    foreach (var step in steps)
                    {
                        if (recurse(step) is Instruction instruction)
                            byteCode.StepInstructions.Add(instruction);
                    }

                    return byteCode;
                })
                .OverrideAtomSerializer<HasNotStep>((step, overridden, recurse) => CreateInstruction("hasNot", recurse, step.Key))
                .OverrideAtomSerializer<ChooseOptionTraversalStep>((step, overridden, recurse) => CreateInstruction("choose", recurse, step.Traversal))
                .OverrideAtomSerializer<OptionTraversalStep>((step, overridden, recurse) => CreateInstruction("option", recurse, step.Guard, step.OptionTraversal))
                .OverrideAtomSerializer<WithoutStrategiesStep>((step, overridden, recurse) =>
                {
                    return CreateInstruction(
                        "withoutStrategies",
                        recurse,
                        step.StrategyTypes);
                })
                .OverrideAtomSerializer<HasStep>((step, overridden, recurse) =>
                {
                    if (step.Value is P p1 && p1.EqualsConstant(false))
                        return recurse(NoneStep.Instance);
                    else
                    {
                        var stepName = "has";
                        var argument = step.Value;

                        if (argument is P p2)
                        {
                            if (p2 is P.SingleArgumentP singleArgumentP)
                            {
                                if (singleArgumentP.Argument == null)
                                {
                                    if (p2 is P.Eq)
                                        stepName = "hasNot";
                                    else if (p2 is P.Neq)
                                        argument = null;
                                }

                                if (p2 is P.Eq)
                                    argument = singleArgumentP.Argument;
                            }
                            else if (p2 == P.True)
                                argument = null;
                        }

                        if (argument != null)
                            return CreateInstruction(stepName, recurse, step.Key, argument);
                        return CreateInstruction(stepName, recurse, step.Key);
                    }
                })
                .OverrideAtomSerializer<RepeatStep>((step, overridden, recurse) => CreateInstruction("repeat", recurse, step.Traversal))
                .OverrideAtomSerializer<SideEffectStep>((step, overridden, recurse) => CreateInstruction("sideEffect", recurse, step.Traversal))
                .OverrideAtomSerializer<ToTraversalStep>((step, overridden, recurse) => CreateInstruction("to", recurse, step.Traversal))
                .OverrideAtomSerializer<UnionStep>((step, overridden, recurse) => CreateInstruction("union", recurse, step.Traversals.ToArray()))
                .OverrideAtomSerializer<UntilStep>((step, overridden, recurse) => CreateInstruction("until", recurse, step.Traversal))
                .OverrideAtomSerializer<ValuesStep>((step, overridden, recurse) => CreateInstruction("values", recurse, step.Keys))
                .OverrideAtomSerializer<VerticesStep>((step, overridden, recurse) => CreateInstruction("vertices", recurse, step.Traversal))
                .OverrideAtomSerializer<WhereTraversalStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Traversal))
                .OverrideAtomSerializer<WithStrategiesStep>((step, overridden, recurse) => CreateInstruction("withStrategies", recurse, step.Traversal))
                .OverrideAtomSerializer<IdStep>((step, overridden, recurse) => CreateInstruction("id", recurse))
                .OverrideAtomSerializer<BarrierStep>((step, overridden, recurse) => CreateInstruction("barrier", recurse))
                .OverrideAtomSerializer<OrderStep>((step, overridden, recurse) => CreateInstruction("order", recurse))
                .OverrideAtomSerializer<CreateStep>((step, overridden, recurse) => CreateInstruction("create", recurse))
                .OverrideAtomSerializer<UnfoldStep>((step, overridden, recurse) => CreateInstruction("unfold", recurse))
                .OverrideAtomSerializer<IdentityStep>((step, overridden, recurse) => CreateInstruction("identity", recurse))
                .OverrideAtomSerializer<EmitStep>((step, overridden, recurse) => CreateInstruction("emit", recurse))
                .OverrideAtomSerializer<DedupStep>((step, overridden, recurse) => CreateInstruction("dedup", recurse))
                .OverrideAtomSerializer<OutVStep>((step, overridden, recurse) => CreateInstruction("outV", recurse))
                .OverrideAtomSerializer<OtherVStep>((step, overridden, recurse) => CreateInstruction("otherV", recurse))
                .OverrideAtomSerializer<InVStep>((step, overridden, recurse) => CreateInstruction("inV", recurse))
                .OverrideAtomSerializer<BothVStep>((step, overridden, recurse) => CreateInstruction("bothV", recurse))
                .OverrideAtomSerializer<DropStep>((step, overridden, recurse) => CreateInstruction("drop", recurse))
                .OverrideAtomSerializer<FoldStep>((step, overridden, recurse) => CreateInstruction("fold", recurse))
                .OverrideAtomSerializer<ExplainStep>((step, overridden, recurse) => CreateInstruction("explain", recurse))
                .OverrideAtomSerializer<ProfileStep>((step, overridden, recurse) => CreateInstruction("profile", recurse))
                .OverrideAtomSerializer<CountStep>((step, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        return CreateInstruction("count", recurse, step.Scope);
                    else
                        return CreateInstruction("count", recurse);
                })
                .OverrideAtomSerializer<BuildStep>((step, overridden, recurse) => CreateInstruction("build", recurse))
                .OverrideAtomSerializer<SumStep>((step, overridden, recurse) => CreateInstruction("sum", recurse, step.Scope))
                .OverrideAtomSerializer<TailStep>((step, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        return CreateInstruction("tail", recurse, step.Scope, step.Count);
                    else
                        return CreateInstruction("tail", recurse, step.Count);
                })
                .OverrideAtomSerializer<SelectStep>((step, overridden, recurse) => CreateInstruction("select", recurse, step.StepLabels))
                .OverrideAtomSerializer<AsStep>((step, overridden, recurse) => CreateInstruction("as", recurse, step.StepLabels))
                .OverrideAtomSerializer<FromLabelStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.StepLabel))
                .OverrideAtomSerializer<ToLabelStep>((step, overridden, recurse) => CreateInstruction("to", recurse, step.StepLabel))
                .OverrideAtomSerializer<TimesStep>((step, overridden, recurse) => CreateInstruction("times", recurse, step.Count))
                .OverrideAtomSerializer<FilterStep>((step, overridden, recurse) => CreateInstruction("filter", recurse, step.Lambda))
                .OverrideAtomSerializer<AggregateStep>((step, overridden, recurse) => CreateInstruction("aggregate", recurse, step.StepLabel))
                .OverrideAtomSerializer<WherePredicateStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Predicate))
                .OverrideAtomSerializer<ByLambdaStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Lambda))
                .OverrideAtomSerializer<SkipStep>((step, overridden, recurse) => CreateInstruction("skip", recurse, step.Count))
                .OverrideAtomSerializer<PropertyStep>((step, overridden, recurse) =>
                {
                    if (T.Id.Equals(step.Key) && !Cardinality.Single.Equals(step.Cardinality.IfNone(Cardinality.Single)))
                        throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

                    if (ReferenceEquals(step.Key, T.Id))
                        return CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).ToArray());

                    return step.Cardinality.Match(
                        c => CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).Prepend(c).ToArray()),
                        () => CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).ToArray()));
                })
                .OverrideAtomSerializer<RangeStep>((step, overridden, recurse) => CreateInstruction("range", recurse, step.Lower, step.Upper))
                .OverrideAtomSerializer<ByMemberStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Key, step.Order))
                .OverrideAtomSerializer<KeyStep>((step, overridden, recurse) => CreateInstruction("key", recurse))
                .OverrideAtomSerializer<PropertiesStep>((step, overridden, recurse) => CreateInstruction("properties", recurse, step.Keys))
                .OverrideAtomSerializer<VStep>((step, overridden, recurse) => CreateInstruction("V", recurse, step.Ids))
                .OverrideAtomSerializer<EStep>((step, overridden, recurse) => CreateInstruction("E", recurse, step.Ids))
                .OverrideAtomSerializer<InjectStep>((step, overridden, recurse) => CreateInstruction("inject", recurse, step.Elements))
                .OverrideAtomSerializer<P.Eq>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Eq(recurse(p.Argument)))
                .OverrideAtomSerializer<P.Between>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Between(recurse(p.Lower), recurse(p.Upper)))
                .OverrideAtomSerializer<P.Gt>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Gt(recurse(p.Argument)))
                .OverrideAtomSerializer<P.Gte>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Gte(recurse(p.Argument)))
                .OverrideAtomSerializer<P.Lt>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Lt(recurse(p.Argument)))
                .OverrideAtomSerializer<P.Lte>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Lte(recurse(p.Argument)))
                .OverrideAtomSerializer<P.Neq>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Neq(recurse(p.Argument)))
                .OverrideAtomSerializer<P.Within>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Within(p.Arguments.Select(recurse).ToArray()))
                .OverrideAtomSerializer<P.Without>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Without(p.Arguments.Select(recurse).ToArray()))
                .OverrideAtomSerializer<P.Outside>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.P.Outside(recurse(p.Lower), recurse(p.Upper)))
                .OverrideAtomSerializer<P.AndP>((p, overridden, recurse) =>
                {
                    return ((Gremlin.Net.Process.Traversal.P)recurse(p.Operand1)).And(((Gremlin.Net.Process.Traversal.P)recurse(p.Operand2)));
                })
                .OverrideAtomSerializer<P.OrP>((p, overridden, recurse) =>
                {
                    return ((Gremlin.Net.Process.Traversal.P)recurse(p.Operand1)).Or(((Gremlin.Net.Process.Traversal.P)recurse(p.Operand2)));
                })
                .OverrideAtomSerializer<TextP.StartingWith>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.TextP.StartingWith(p.Value))
                .OverrideAtomSerializer<TextP.EndingWith>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.TextP.EndingWith(p.Value))
                .OverrideAtomSerializer<TextP.Containing>((p, overridden, recurse) => Gremlin.Net.Process.Traversal.TextP.Containing(p.Value))
                .OverrideAtomSerializer<Lambda>((lambda, overridden, recurse) => Gremlin.Net.Process.Traversal.Lambda.Groovy(lambda.LambdaString))
                .OverrideAtomSerializer<Cardinality>((enumValue, overridden, recurse) => Gremlin.Net.Process.Traversal.Cardinality.GetByValue(enumValue.Name))
                .OverrideAtomSerializer<Order>((enumValue, overridden, recurse) => Gremlin.Net.Process.Traversal.Order.GetByValue(enumValue.Name))
                .OverrideAtomSerializer<Scope>((enumValue, overridden, recurse) => Gremlin.Net.Process.Traversal.Scope.GetByValue(enumValue.Name))
                .OverrideAtomSerializer<T>((enumValue, overridden, recurse) => Gremlin.Net.Process.Traversal.T.GetByValue(enumValue.Name))
                .OverrideAtomSerializer<HasValueStep>((step, overridden, recurse) =>
                {
                    return CreateInstruction(
                        "hasValue",
                        recurse,
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument);
                })
                .OverrideAtomSerializer<AddEStep>((step, overridden, recurse) => CreateInstruction("addE", recurse, step.Label))
                .OverrideAtomSerializer<AddVStep>((step, overridden, recurse) => CreateInstruction("addV", recurse, step.Label))
                .OverrideAtomSerializer<AndStep>((step, overridden, recurse) => CreateInstruction("and", recurse, step.Traversals.SelectMany(FlattenLogicalTraversals<AndStep>).ToArray()))
                .OverrideAtomSerializer<ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal, step.Order))
                .OverrideAtomSerializer<ChooseTraversalStep>((step, overridden, recurse) =>
                {
                    return step.ElseTraversal.Match(
                        elseTraversal => CreateInstruction(
                            "choose",
                            recurse,
                            step.IfTraversal,
                            step.ThenTraversal,
                            elseTraversal),
                        () => CreateInstruction(
                            "choose",
                            recurse,
                            step.IfTraversal,
                            step.ThenTraversal));
                })
                .OverrideAtomSerializer<ChoosePredicateStep>((step, overridden, recurse) =>
                {
                    return step.ElseTraversal.Match(
                        elseTraversal => CreateInstruction(
                            "choose",
                            recurse,
                            step.Predicate,
                            step.ThenTraversal,
                            elseTraversal),
                        () => CreateInstruction(
                            "choose",
                            recurse,
                            step.Predicate,
                            step.ThenTraversal));
                })
                .OverrideAtomSerializer<CoalesceStep>((step, overridden, recurse) => CreateInstruction("coalesce", recurse, step.Traversals.ToArray()))
                .OverrideAtomSerializer<CoinStep>((step, overridden, recurse) => CreateInstruction("coin", recurse, step.Probability))
                .OverrideAtomSerializer<ConstantStep>((step, overridden, recurse) => CreateInstruction("constant", recurse, step.Value))
                .OverrideAtomSerializer<BothStep>((step, overridden, recurse) => CreateInstruction("both", recurse, step.Labels))
                .OverrideAtomSerializer<BothEStep>((step, overridden, recurse) => CreateInstruction("bothE", recurse, step.Labels))
                .OverrideAtomSerializer<InStep>((step, overridden, recurse) => CreateInstruction("in", recurse, step.Labels))
                .OverrideAtomSerializer<InEStep>((step, overridden, recurse) => CreateInstruction("inE", recurse, step.Labels))
                .OverrideAtomSerializer<OutStep>((step, overridden, recurse) => CreateInstruction("out", recurse, step.Labels))
                .OverrideAtomSerializer<OutEStep>((step, overridden, recurse) => CreateInstruction("outE", recurse, step.Labels))
                .OverrideAtomSerializer<HasLabelStep>((step, overridden, recurse) => CreateInstruction("hasLabel", recurse, step.Labels))
                .OverrideAtomSerializer<LabelStep>((step, overridden, recurse) => CreateInstruction("label", recurse))
                .OverrideAtomSerializer<EdgesStep>((step, overridden, recurse) => CreateInstruction("edges", recurse, step.Traversal))
                .OverrideAtomSerializer<FromTraversalStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.Traversal))
                .OverrideAtomSerializer<IsStep>((step, overridden, recurse) =>
                {
                    return CreateInstruction(
                        "is",
                        recurse,
                        step.Argument is P.Eq eq
                            ? eq.Argument
                            : step.Argument);
                })
                .OverrideAtomSerializer<LimitStep>((step, overridden, recurse) =>
                {
                    if (step.Scope.Equals(Scope.Local))
                        return CreateInstruction("limit", recurse, step.Scope, step.Count);
                    else
                        return CreateInstruction("limit", recurse, step.Count);
                })
                .OverrideAtomSerializer<LocalStep>((step, overridden, recurse) => CreateInstruction("local", recurse, step.Traversal))
                .OverrideAtomSerializer<MapStep>((step, overridden, recurse) => CreateInstruction("map", recurse, step.Traversal))
                //.OverrideAtomSerializer<NoneStep>((step, overridden, recurse) => CreateInstruction("none"))
                .OverrideAtomSerializer<NoneStep>((step, overridden, recurse) => recurse(NoneWorkaround))
                .OverrideAtomSerializer<FlatMapStep>((step, overridden, recurse) => CreateInstruction("flatMap", recurse, step.Traversal))
                .OverrideAtomSerializer<MatchStep>((step, overridden, recurse) => CreateInstruction("match", recurse, step.Traversals.ToArray()))
                .OverrideAtomSerializer<NotStep>((step, overridden, recurse) =>
                {
                    var traversalSteps = step.Traversal.AsAdmin().Steps;

                    if (!(traversalSteps.Count > 0 && traversalSteps[traversalSteps.Count - 1] is HasStep hasStep && hasStep.Value is P p && p.EqualsConstant(false)))
                        return CreateInstruction("not", recurse, step.Traversal);

                    return null;
                })
                .OverrideAtomSerializer<OptionalStep>((step, overridden, recurse) => CreateInstruction("optional", recurse, step.Traversal))
                .OverrideAtomSerializer<OrStep>((step, overridden, recurse) => CreateInstruction("or", recurse, step.Traversals.SelectMany(FlattenLogicalTraversals<OrStep>).ToArray()))
                .OverrideAtomSerializer<ValueStep>((step, overridden, recurse) => CreateInstruction("value", recurse))
                .OverrideAtomSerializer<ValueMapStep>((step, overridden, recurse) => CreateInstruction("valueMap", recurse, step.Keys))
                .OverrideAtomSerializer<ProjectStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                .OverrideAtomSerializer<ProjectStep>((step, overridden, recurse) => CreateInstruction("project", recurse, step.Projections))
                .OverrideAtomSerializer<Type>((type, overridden, recurse) => type);
        }

        public static IGremlinQuerySerializer Select(this IGremlinQuerySerializer serializer, Func<object, object> projection)
        {
            return new SelectGremlinQuerySerializer(serializer, projection);
        }

        public static IGremlinQuerySerializer ToGroovy(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .OverrideAtomSerializer<IGremlinQuery>((query, overridden, recurse) => (query.Identifier, overridden(query)))
                .Select(serialized =>
                {
                    var builder = new StringBuilder();
                    var variables = new Dictionary<string, object>();

                    void Append(object obj)
                    {
                        if (obj is ValueTuple<string, object> tuple)
                        {
                            builder.Append(tuple.Item1);
                            Append(tuple.Item2);
                        }
                        if (obj is Bytecode bytecode)
                        {
                            //builder.Append(builder.Length == 0 ? "g" : "__");

                            foreach (var instruction in bytecode.StepInstructions)
                            {
                                builder.Append($".{instruction.OperatorName}(");

                                Append(instruction.Arguments);

                                builder.Append(")");
                            }
                        }
                        else if (obj is Binding binding)
                        {
                            builder.Append(binding.Key);
                            variables[binding.Key] = binding.Value;
                        }
                        else if (obj is Gremlin.Net.Process.Traversal.P p)
                        {
                            if (p.Value is Gremlin.Net.Process.Traversal.P p1)
                            {
                                Append(p1);
                                builder.Append($".{p.OperatorName}(");
                                Append(p.Other);

                                builder.Append(")");
                            }
                            else
                            {
                                builder.Append($"{p.OperatorName}(");

                                Append(p.Value);

                                builder.Append(")");
                            }

                        }
                        else if (obj is EnumWrapper t)
                        {
                            builder.Append($"{t.EnumValue}");
                        }
                        else if (obj is ILambda lambda)
                        {
                            builder.Append($"{{{lambda.LambdaExpression}}}");
                        }
                        else if (obj is string str)
                        {
                            builder.Append($"'{str}'");
                        }
                        else if (obj is Type type)
                        {
                            builder.Append(type.Name);
                        }
                        else if (obj is IEnumerable enumerable)
                        {
                            var comma = false;
                            foreach (var argument in enumerable)
                            {
                                if (comma)
                                    builder.Append(", ");
                                else
                                    comma = true;

                                Append(argument);
                            }
                        }
                    }

                    Append(serialized);

                    return new GroovySerializedGremlinQuery(
                        builder.ToString(),
                        variables);
                });
        }

        private static Instruction CreateInstruction(string name, Func<object, object> recurse, params object[] parameters)
        {
            return new Instruction(
                name,
                parameters
                    .Select(recurse)
                    .ToArray());
        }

        private static IEnumerable<IGremlinQuery> FlattenLogicalTraversals<TStep>(IGremlinQuery query) where TStep : LogicalStep
        {
            var steps = query.AsAdmin().Steps;

            if (steps.Count == 1 && steps[0] is TStep otherStep)
            {
                foreach (var subTraversal in otherStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenLogicalTraversals<TStep>(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return query;
        }
    }
}
