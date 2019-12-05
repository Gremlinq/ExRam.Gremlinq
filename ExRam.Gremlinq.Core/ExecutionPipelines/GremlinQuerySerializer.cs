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
            private readonly IImmutableDictionary<Type, QueryFragmentSerializer<object>> _dict;
            private readonly Lazy<ConcurrentDictionary<Type, QueryFragmentSerializer<object>?>> _lazyFastDict;

            public GremlinQuerySerializerImpl(IImmutableDictionary<Type, QueryFragmentSerializer<object>> dict)
            {
                _dict = dict;
                _lazyFastDict = new Lazy<ConcurrentDictionary<Type, QueryFragmentSerializer<object>?>>(
                    () => new ConcurrentDictionary<Type, QueryFragmentSerializer<object>?>(dict.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),
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
                    if (ReferenceEquals(o, null))
                        return o;

                    var action = GetSerializer(o.GetType()) ?? Constant;

                    return action(o, _ => throw new NotImplementedException(), RecurseImpl);
                }

                return RecurseImpl(query);
            }

            public IGremlinQuerySerializer OverrideFragmentSerializer<TAtom>(QueryFragmentSerializer<TAtom> queryFragmentSerializer)
            {
                return new GremlinQuerySerializerImpl(
                    _dict
                        .TryGetValue(typeof(TAtom))
                        .Match(
                            existingAtomSerializer => _dict.SetItem(typeof(TAtom), (atom, baseSerializer, recurse) => queryFragmentSerializer((TAtom)atom, _ => existingAtomSerializer(_, baseSerializer, recurse), recurse)),
                            () => _dict.SetItem(typeof(TAtom), (atom, baseSerializer, recurse) => queryFragmentSerializer((TAtom)atom, _ => throw new NotImplementedException(), recurse))));
            }

            private QueryFragmentSerializer<object>? GetSerializer(Type type)
            {
                return _lazyFastDict.Value
                    .GetOrAdd(
                        type,
                        closureType =>
                        {
                            foreach (var implementedInterface in closureType.GetInterfaces())
                            {
                                if (GetSerializer(implementedInterface) is QueryFragmentSerializer<object> interfaceSerializer)
                                    return interfaceSerializer;
                            }

                            if (closureType.BaseType is Type baseType)
                            {
                                if (GetSerializer(baseType) is QueryFragmentSerializer<object> baseSerializer)
                                    return baseSerializer;
                            }

                            return null;
                        });
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public IGremlinQuerySerializer OverrideFragmentSerializer<TAtom>(QueryFragmentSerializer<TAtom> queryFragmentSerializer)
            {
                throw new InvalidOperationException($"{nameof(OverrideFragmentSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
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

            public IGremlinQuerySerializer OverrideFragmentSerializer<TAtom>(QueryFragmentSerializer<TAtom> queryFragmentSerializer)
            {
                return new SelectGremlinQuerySerializer(_baseSerializer.OverrideFragmentSerializer(queryFragmentSerializer), _projection);
            }

            public object Serialize(IGremlinQuery query)
            {
                return _projection(_baseSerializer.Serialize(query));
            }
        }

        public static readonly GremlinqOption<bool> WorkaroundTinkerpop2112 = new GremlinqOption<bool>(false);

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Unit = new GremlinQuerySerializerImpl(ImmutableDictionary<Type, QueryFragmentSerializer<object>>.Empty);

        public static readonly IGremlinQuerySerializer Default = Unit
            .UseDefaultGremlinStepSerializationHandlers();

        private static readonly Step NoneWorkaround = new NotStep(GremlinQuery.Anonymous(GremlinQueryEnvironment.Default).Identity());

        private static readonly Instruction[] VertexProjectionInstructions;
        private static readonly Instruction[] EdgeProjectionInstructions;

        static GremlinQuerySerializer()
        {
            VertexProjectionInstructions = new[]
            {
                new Instruction("project", "id", "label", "type", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
                new Instruction("by", new Bytecode
                {
                    StepInstructions =
                    {
                        new Instruction("constant", "vertex")
                    }
                }),
                new Instruction(
                    "by",
                    new Bytecode
                    {
                        StepInstructions =
                        {
                            new Instruction("properties"),
                            new Instruction("group"),
                            new Instruction("by", T.Label),
                            new Instruction("by", new Bytecode
                            {
                                StepInstructions =
                                {
                                    new Instruction("project", "id", "label", "value", "properties"),
                                    new Instruction("by", T.Id),
                                    new Instruction("by", T.Label),
                                    new Instruction("by", T.Value),
                                    new Instruction("by", new Bytecode
                                    {
                                        StepInstructions =
                                        {
                                            new Instruction("valueMap")
                                        }
                                    }),
                                    new Instruction("fold")
                                }
                            })
                        }
                    })
            };

            EdgeProjectionInstructions = new[]
            {
                new Instruction("project", "id", "label", "type", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
                new Instruction("by", new Bytecode
                {
                    StepInstructions =
                    {
                        new Instruction("constant", "edge")
                    }
                }),
                new Instruction("by", new Bytecode
                {
                    StepInstructions =
                    {
                        new Instruction("valueMap")
                    }
                })
            };
        }

        public static IGremlinQuerySerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .OverrideFragmentSerializer<AddEStep>((step, overridden, recurse) => CreateInstruction("addE", recurse, step.Label))
                .OverrideFragmentSerializer<AddEStep.ToLabelStep>((step, overridden, recurse) => CreateInstruction("to", recurse, step.StepLabel))
                .OverrideFragmentSerializer<AddEStep.ToTraversalStep>((step, overridden, recurse) => CreateInstruction("to", recurse, step.Traversal))
                .OverrideFragmentSerializer<AddVStep>((step, overridden, recurse) => CreateInstruction("addV", recurse, step.Label))
                .OverrideFragmentSerializer<AndStep>((step, overridden, recurse) => CreateInstruction("and", recurse, step.Traversals.SelectMany(FlattenLogicalTraversals<AndStep>).ToArray()))
                .OverrideFragmentSerializer<AggregateStep>((step, overridden, recurse) => CreateInstruction("aggregate", recurse, step.StepLabel))
                .OverrideFragmentSerializer<AsStep>((step, overridden, recurse) => CreateInstruction("as", recurse, step.StepLabels))
                .OverrideFragmentSerializer<BarrierStep>((step, overridden, recurse) => CreateInstruction("barrier", recurse))
                .OverrideFragmentSerializer<BothStep>((step, overridden, recurse) => CreateInstruction("both", recurse, step.Labels))
                .OverrideFragmentSerializer<BothEStep>((step, overridden, recurse) => CreateInstruction("bothE", recurse, step.Labels))
                .OverrideFragmentSerializer<BothVStep>((step, overridden, recurse) => CreateInstruction("bothV", recurse))
                .OverrideFragmentSerializer<BuildStep>((step, overridden, recurse) => CreateInstruction("build", recurse))
                .OverrideFragmentSerializer<ChooseOptionTraversalStep>((step, overridden, recurse) => CreateInstruction("choose", recurse, step.Traversal))
                .OverrideFragmentSerializer<ChoosePredicateStep>((step, overridden, recurse) =>
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
                .OverrideFragmentSerializer<ChooseTraversalStep>((step, overridden, recurse) =>
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
                .OverrideFragmentSerializer<CoalesceStep>((step, overridden, recurse) => CreateInstruction("coalesce", recurse, step.Traversals.ToArray()))
                .OverrideFragmentSerializer<CoinStep>((step, overridden, recurse) => CreateInstruction("coin", recurse, step.Probability))
                .OverrideFragmentSerializer<ConstantStep>((step, overridden, recurse) => CreateInstruction("constant", recurse, step.Value))
                .OverrideFragmentSerializer<CountStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local) ? CreateInstruction("count", recurse, step.Scope) : CreateInstruction("count", recurse))
                .OverrideFragmentSerializer<CreateStep>((step, overridden, recurse) => CreateInstruction("create", recurse))
                .OverrideFragmentSerializer<DedupStep>((step, overridden, recurse) => CreateInstruction("dedup", recurse))
                .OverrideFragmentSerializer<DropStep>((step, overridden, recurse) => CreateInstruction("drop", recurse))
                .OverrideFragmentSerializer<EdgesStep>((step, overridden, recurse) => CreateInstruction("edges", recurse, step.Traversal))
                .OverrideFragmentSerializer<EmitStep>((step, overridden, recurse) => CreateInstruction("emit", recurse))
                .OverrideFragmentSerializer<EnumWrapper>((enumValue, overridden, recurse) => enumValue)
                .OverrideFragmentSerializer<EStep>((step, overridden, recurse) => CreateInstruction("E", recurse, step.Ids))
                .OverrideFragmentSerializer<ExplainStep>((step, overridden, recurse) => CreateInstruction("explain", recurse))
                .OverrideFragmentSerializer<FoldStep>((step, overridden, recurse) => CreateInstruction("fold", recurse))
                .OverrideFragmentSerializer<FilterStep>((step, overridden, recurse) => CreateInstruction("filter", recurse, step.Lambda))
                .OverrideFragmentSerializer<FlatMapStep>((step, overridden, recurse) => CreateInstruction("flatMap", recurse, step.Traversal))
                .OverrideFragmentSerializer<FromLabelStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.StepLabel))
                .OverrideFragmentSerializer<FromTraversalStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.Traversal))
                .OverrideFragmentSerializer<GroupStep>((step, overridden, recurse) => CreateInstruction("group", recurse))
                .OverrideFragmentSerializer<GroupStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                .OverrideFragmentSerializer<HasStep>((step, overridden, recurse) =>
                {
                    if (step.Value is P p1 && p1.EqualsConstant(false))
                        return recurse(NoneStep.Instance);

                    var stepName = "has";
                    var argument = (object?)step.Value;

                    if (argument is P p2)
                    {
                        if (p2.EqualsConstant(true))
                            argument = null;
                        else
                        {
                            if (p2.Value == null)
                            {
                                if (p2.OperatorName == "eq")
                                    stepName = "hasNot";
                                else if (p2.OperatorName == "neq")
                                    argument = null;
                            }

                            if (p2.OperatorName == "eq")
                                argument = p2.Value;
                        }
                    }

                    return argument != null
                        ? CreateInstruction(stepName, recurse, step.Key, argument)
                        : CreateInstruction(stepName, recurse, step.Key);
                })
                .OverrideFragmentSerializer<HasLabelStep>((step, overridden, recurse) => CreateInstruction("hasLabel", recurse, step.Labels))
                .OverrideFragmentSerializer<HasNotStep>((step, overridden, recurse) => CreateInstruction("hasNot", recurse, step.Key))
                .OverrideFragmentSerializer<HasValueStep>((step, overridden, recurse) => CreateInstruction(
                    "hasValue",
                    recurse,
                    step.Argument is P p && p.OperatorName == "eq"
                        ? p.Value
                        : step.Argument))
                .OverrideFragmentSerializer<IdentityStep>((step, overridden, recurse) => CreateInstruction("identity", recurse))
                .OverrideFragmentSerializer<IdStep>((step, overridden, recurse) => CreateInstruction("id", recurse))
                .OverrideFragmentSerializer<IGremlinQuery>((query, overridden, recurse) =>
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

                    if (query is GremlinQueryBase gremlinQueryBase && gremlinQueryBase.SurfaceVisible)
                    {
                        var model = query.AsAdmin().Environment.Model;
                        var elementType = query.GetType().GetGenericArguments()[0];

                        if (model.VerticesModel.Metadata.ContainsKey(elementType))
                            byteCode.StepInstructions.AddRange(VertexProjectionInstructions);
                        else if (model.EdgesModel.Metadata.ContainsKey(elementType))
                            byteCode.StepInstructions.AddRange(EdgeProjectionInstructions);
                    }

                    return byteCode;
                })
                .OverrideFragmentSerializer<ILambda>((lambda, overridden, recurse) => lambda)
                .OverrideFragmentSerializer<InjectStep>((step, overridden, recurse) => CreateInstruction("inject", recurse, step.Elements))
                .OverrideFragmentSerializer<InEStep>((step, overridden, recurse) => CreateInstruction("inE", recurse, step.Labels))
                .OverrideFragmentSerializer<InStep>((step, overridden, recurse) => CreateInstruction("in", recurse, step.Labels))
                .OverrideFragmentSerializer<InVStep>((step, overridden, recurse) => CreateInstruction("inV", recurse))
                .OverrideFragmentSerializer<IsStep>((step, overridden, recurse) => CreateInstruction(
                    "is",
                    recurse,
                    step.Argument is P p && p.OperatorName == "eq"
                        ? p.Value
                        : step.Argument))
                .OverrideFragmentSerializer<KeyStep>((step, overridden, recurse) => CreateInstruction("key", recurse))
                .OverrideFragmentSerializer<LabelStep>((step, overridden, recurse) => CreateInstruction("label", recurse))
                .OverrideFragmentSerializer<LimitStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("limit", recurse, step.Scope, step.Count)
                    : CreateInstruction("limit", recurse, step.Count))
                .OverrideFragmentSerializer<LocalStep>((step, overridden, recurse) => CreateInstruction("local", recurse, step.Traversal))
                .OverrideFragmentSerializer<MatchStep>((step, overridden, recurse) => CreateInstruction("match", recurse, step.Traversals.ToArray()))
                .OverrideFragmentSerializer<MapStep>((step, overridden, recurse) => CreateInstruction("map", recurse, step.Traversal))
                .OverrideFragmentSerializer<NoneStep>((step, overridden, recurse) => recurse(NoneWorkaround))
                .OverrideFragmentSerializer<NotStep>((step, overridden, recurse) =>
                {
                    var traversalSteps = step.Traversal.AsAdmin().Steps;

                    return !(traversalSteps.Count > 0 && traversalSteps[traversalSteps.Count - 1] is HasStep hasStep && hasStep.Value is P p && p.EqualsConstant(false))
                        ? CreateInstruction("not", recurse, step.Traversal)
                        : null;
                })
                .OverrideFragmentSerializer<OptionalStep>((step, overridden, recurse) => CreateInstruction("optional", recurse, step.Traversal))
                .OverrideFragmentSerializer<OptionTraversalStep>((step, overridden, recurse) => CreateInstruction("option", recurse, step.Guard.IfNone(Pick.None), step.OptionTraversal))
                .OverrideFragmentSerializer<OrderStep>((step, overridden, recurse) => CreateInstruction("order", recurse))
                .OverrideFragmentSerializer<OrderStep.ByLambdaStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Lambda))
                .OverrideFragmentSerializer<OrderStep.ByMemberStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Key, step.Order))
                .OverrideFragmentSerializer<OrderStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal, step.Order))
                .OverrideFragmentSerializer<OrStep>((step, overridden, recurse) => CreateInstruction("or", recurse, step.Traversals.SelectMany(FlattenLogicalTraversals<OrStep>).ToArray()))
                .OverrideFragmentSerializer<OutStep>((step, overridden, recurse) => CreateInstruction("out", recurse, step.Labels))
                .OverrideFragmentSerializer<OutEStep>((step, overridden, recurse) => CreateInstruction("outE", recurse, step.Labels))
                .OverrideFragmentSerializer<OutVStep>((step, overridden, recurse) => CreateInstruction("outV", recurse))
                .OverrideFragmentSerializer<OtherVStep>((step, overridden, recurse) => CreateInstruction("otherV", recurse))
                .OverrideFragmentSerializer<P>((p, overridden, recurse) =>
                {
                    //TODO: Have the array bound!
                    if (!(p.Value is string) && p.Value is IEnumerable enumerable)
                        return new P(p.OperatorName, enumerable.Cast<object>().Select(recurse).ToArray(), (P)recurse(p.Other));

                    return new P(p.OperatorName, recurse(p.Value), (P)recurse(p.Other));
                })
                .OverrideFragmentSerializer<ProfileStep>((step, overridden, recurse) => CreateInstruction("profile", recurse))
                .OverrideFragmentSerializer<PropertiesStep>((step, overridden, recurse) => CreateInstruction("properties", recurse, step.Keys))
                .OverrideFragmentSerializer<PropertyStep>((step, overridden, recurse) =>
                {
                    if (T.Id.Equals(step.Key))
                    {
                        if (!Cardinality.Single.Equals(step.Cardinality.IfNone(Cardinality.Single)))
                            throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

                        return CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).ToArray());
                    }

                    return step.Cardinality.Match(
                        c => CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).Prepend(c).ToArray()),
                        () => CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).ToArray()));
                })
                .OverrideFragmentSerializer<ProjectStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                .OverrideFragmentSerializer<ProjectStep>((step, overridden, recurse) => CreateInstruction("project", recurse, step.Projections))
                .OverrideFragmentSerializer<RangeStep>((step, overridden, recurse) => CreateInstruction("range", recurse, step.Lower, step.Upper))
                .OverrideFragmentSerializer<RepeatStep>((step, overridden, recurse) => CreateInstruction("repeat", recurse, step.Traversal))
                .OverrideFragmentSerializer<SelectStep>((step, overridden, recurse) => CreateInstruction("select", recurse, step.StepLabels))
                .OverrideFragmentSerializer<SideEffectStep>((step, overridden, recurse) => CreateInstruction("sideEffect", recurse, step.Traversal))
                .OverrideFragmentSerializer<SkipStep>((step, overridden, recurse) => CreateInstruction("skip", recurse, step.Count))
                .OverrideFragmentSerializer<SumStep>((step, overridden, recurse) => CreateInstruction("sum", recurse, step.Scope))
                .OverrideFragmentSerializer<TailStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local) ? CreateInstruction("tail", recurse, step.Scope, step.Count) : CreateInstruction("tail", recurse, step.Count))
                .OverrideFragmentSerializer<TimesStep>((step, overridden, recurse) => CreateInstruction("times", recurse, step.Count))
                .OverrideFragmentSerializer<Type>((type, overridden, recurse) => type)
                .OverrideFragmentSerializer<UnfoldStep>((step, overridden, recurse) => CreateInstruction("unfold", recurse))
                .OverrideFragmentSerializer<UnionStep>((step, overridden, recurse) => CreateInstruction("union", recurse, step.Traversals.ToArray()))
                .OverrideFragmentSerializer<UntilStep>((step, overridden, recurse) => CreateInstruction("until", recurse, step.Traversal))
                .OverrideFragmentSerializer<ValueStep>((step, overridden, recurse) => CreateInstruction("value", recurse))
                .OverrideFragmentSerializer<ValueMapStep>((step, overridden, recurse) => CreateInstruction("valueMap", recurse, step.Keys))
                .OverrideFragmentSerializer<ValuesStep>((step, overridden, recurse) => CreateInstruction("values", recurse, step.Keys))
                .OverrideFragmentSerializer<VerticesStep>((step, overridden, recurse) => CreateInstruction("vertices", recurse, step.Traversal))
                .OverrideFragmentSerializer<VStep>((step, overridden, recurse) => CreateInstruction("V", recurse, step.Ids))
                .OverrideFragmentSerializer<WhereTraversalStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Traversal))
                .OverrideFragmentSerializer<WithStrategiesStep>((step, overridden, recurse) => CreateInstruction("withStrategies", recurse, step.Traversal))
                .OverrideFragmentSerializer<WithoutStrategiesStep>((step, overridden, recurse) => CreateInstruction("withoutStrategies", recurse, step.StrategyTypes))
                .OverrideFragmentSerializer<WherePredicateStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Predicate));
        }

        public static IGremlinQuerySerializer Select(this IGremlinQuerySerializer serializer, Func<object, object> projection)
        {
            return new SelectGremlinQuerySerializer(serializer, projection);
        }

        public static IGremlinQuerySerializer ToGroovy(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .Select(serialized =>
                {
                    var builder = new StringBuilder();
                    var variables = new Dictionary<string, object>();

                    void Append(object obj)
                    {
                        if (obj is GroovySerializedGremlinQuery serializedQuery)
                            builder.Append(serializedQuery.QueryString);
                        else if (obj is Bytecode bytecode)
                        {
                            if (builder.Length > 0)
                                builder.Append("__");

                            foreach (var instruction in bytecode.StepInstructions)
                            {
                                builder.Append(builder.Length != 0
                                    ? $".{instruction.OperatorName}("
                                    : $"{instruction.OperatorName}(");

                                Append(instruction.Arguments);

                                builder.Append(")");
                            }
                        }
                        else if (obj is Binding binding)
                        {
                            builder.Append(binding.Key);
                            variables[binding.Key] = binding.Value;
                        }
                        else if (obj is P p)
                        {
                            if (p.Value is P p1)
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
