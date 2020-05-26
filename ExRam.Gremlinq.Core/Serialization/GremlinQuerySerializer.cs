using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer
    {
        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            [ThreadStatic]
            private static Dictionary<StepLabel, string>? _stepLabelNames;

            private readonly IQueryFragmentSerializer _fragmentSerializer;
            private readonly IQueryFragmentSerializer _originalfragmentSerializer;

            public GremlinQuerySerializerImpl(IQueryFragmentSerializer fragmentSerializer)
            {
                _originalfragmentSerializer = fragmentSerializer;

                _fragmentSerializer = fragmentSerializer
                    .Override<StepLabel>((stepLabel, @base, recurse) =>
                    {
                        string? stepLabelMapping = null;

                        if (!_stepLabelNames!.TryGetValue(stepLabel, out stepLabelMapping))
                        {
                            stepLabelMapping = "l" + (_stepLabelNames.Count + 1);
                            _stepLabelNames.Add(stepLabel, stepLabelMapping);
                        }

                        // ReSharper disable once TailRecursiveCall
                        return recurse.Serialize(stepLabelMapping!);
                    });
            }

            public object Serialize(IGremlinQueryBase query)
            {
                (_stepLabelNames ??= new Dictionary<StepLabel, string>()).Clear();

                return _fragmentSerializer
                    .Serialize(query);
            }

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IQueryFragmentSerializer, IQueryFragmentSerializer> transformation)
            {
                return new GremlinQuerySerializerImpl(transformation(_originalfragmentSerializer));
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public object Serialize(IGremlinQueryBase query) => throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IQueryFragmentSerializer, IQueryFragmentSerializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
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

            public object Serialize(IGremlinQueryBase query) => _projection(_baseSerializer.Serialize(query));

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IQueryFragmentSerializer, IQueryFragmentSerializer> transformation) => new SelectGremlinQuerySerializer(_baseSerializer.ConfigureFragmentSerializer(transformation), _projection);
        }

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Identity = new GremlinQuerySerializerImpl(
            QueryFragmentSerializer.Identity);

        public static readonly IGremlinQuerySerializer Default = Identity
            .UseDefaultGremlinStepSerializationHandlers();

        private static readonly ImmutableArray<Step> IdentitySteps = ImmutableArray.Create((Step)IdentityStep.Instance);
        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new ConcurrentDictionary<string, Instruction>();

        static GremlinQuerySerializer()
        {
        }

        public static IGremlinQuerySerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer
                    .Override<AddEStep>((step, overridden, recurse) => CreateInstruction("addE", recurse, step.Label))
                    .Override<AddEStep.ToLabelStep>((step, overridden, recurse) => CreateInstruction("to", recurse, step.StepLabel))
                    .Override<AddEStep.ToTraversalStep>((step, overridden, recurse) => CreateInstruction("to", recurse, step.Traversal))
                    .Override<AddEStep.FromLabelStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.StepLabel))
                    .Override<AddEStep.FromTraversalStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.Traversal))
                    .Override<AddVStep>((step, overridden, recurse) => CreateInstruction("addV", recurse, step.Label))
                    .Override<AndStep>((step, overridden, recurse) => CreateInstruction("and", recurse, step.Traversals))
                    .Override<AggregateStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("aggregate", recurse, step.StepLabel)
                        : CreateInstruction("aggregate", recurse, step.Scope, step.StepLabel))
                    .Override<AsStep>((step, overridden, recurse) => CreateInstruction("as", recurse, step.StepLabel))
                    .Override<BarrierStep>((step, overridden, recurse) => CreateInstruction("barrier"))
                    .Override<BothStep>((step, overridden, recurse) => CreateInstruction("both", recurse, step.Labels))
                    .Override<BothEStep>((step, overridden, recurse) => CreateInstruction("bothE", recurse, step.Labels))
                    .Override<BothVStep>((step, overridden, recurse) => CreateInstruction("bothV"))
                    .Override<CapStep>((step, overridden, recurse) => CreateInstruction("cap", recurse, step.StepLabel))
                    .Override<ChooseOptionTraversalStep>((step, overridden, recurse) => CreateInstruction("choose", recurse, step.Traversal))
                    .Override<ChoosePredicateStep>((step, overridden, recurse) =>
                    {
                        return step.ElseTraversal is { } elseTraversal
                            ? CreateInstruction(
                                "choose",
                                recurse,
                                step.Predicate,
                                step.ThenTraversal,
                                elseTraversal)
                            : CreateInstruction(
                                "choose",
                                recurse,
                                step.Predicate,
                                step.ThenTraversal);
                    })
                    .Override<ChooseTraversalStep>((step, overridden, recurse) =>
                    {
                        return step.ElseTraversal is { } elseTraversal
                            ? CreateInstruction(
                                "choose",
                                recurse,
                                step.IfTraversal,
                                step.ThenTraversal,
                                elseTraversal)
                            : CreateInstruction(
                                "choose",
                                recurse,
                                step.IfTraversal,
                                step.ThenTraversal);
                    })
                    .Override<CoalesceStep>((step, overridden, recurse) => CreateInstruction("coalesce", recurse, step.Traversals))
                    .Override<CoinStep>((step, overridden, recurse) => CreateInstruction("coin", recurse, step.Probability))
                    .Override<ConstantStep>((step, overridden, recurse) => CreateInstruction("constant", recurse, step.Value))
                    .Override<CountStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("count", recurse, step.Scope)
                        : CreateInstruction("count"))
                    .Override<DateTime>((dateTime, overridden, recurse) => recurse.Serialize(new DateTimeOffset(dateTime.ToUniversalTime())))
                    .Override<DedupStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("dedup", recurse, step.Scope)
                        : CreateInstruction("dedup"))
                    .Override<DropStep>((step, overridden, recurse) => CreateInstruction("drop"))
                    .Override<EmitStep>((step, overridden, recurse) => CreateInstruction("emit"))
                    .Override<EnumWrapper>((enumValue, overridden, recurse) => enumValue)
                    .Override<EStep>((step, overridden, recurse) => CreateInstruction("E", recurse, step.Ids))
                    .Override<Expression>((expression, overridden, recurse) => recurse.Serialize(expression.GetValue()))
                    .Override<ExplainStep>((step, overridden, recurse) => CreateInstruction("explain"))
                    .Override<FoldStep>((step, overridden, recurse) => CreateInstruction("fold"))
                    .Override<FilterStep>((step, overridden, recurse) => CreateInstruction("filter", recurse, step.Lambda))
                    .Override<FlatMapStep>((step, overridden, recurse) => CreateInstruction("flatMap", recurse, step.Traversal))
                    .Override<GroupStep>((step, overridden, recurse) => CreateInstruction("group"))
                    .Override<GroupStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                    .Override<GroupStep.ByKeyStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Key))
                    .Override<HasKeyStep>((step, overridden, recurse) => CreateInstruction(
                        "hasKey",
                        recurse,
                        step.Argument is P p && p.OperatorName == "eq"
                            ? p.Value
                            : step.Argument))
                    .Override<HasPredicateStep>((step, overridden, recurse) =>
                    {
                        var stepName = "has";
                        var argument = (object?)step.Predicate;

                        if (argument is P p2)
                        {
                            if (p2.Value == null)
                            {
                                argument = null;

                                if (p2.OperatorName == "eq")
                                    stepName = "hasNot";
                            }
                            else if (p2.OperatorName == "eq")
                                argument = p2.Value;
                        }

                        return argument != null
                            ? CreateInstruction(stepName, recurse, step.Key, argument)
                            : CreateInstruction(stepName, recurse, step.Key);
                    })
                    .Override<HasTraversalStep>((step, overridden, recurse) => CreateInstruction("has", recurse, step.Key, step.Traversal))
                    .Override<HasLabelStep>((step, overridden, recurse) => CreateInstruction("hasLabel", recurse, step.Labels))
                    .Override<HasNotStep>((step, overridden, recurse) => CreateInstruction("hasNot", recurse, step.Key))
                    .Override<HasValueStep>((step, overridden, recurse) => CreateInstruction(
                        "hasValue",
                        recurse,
                        step.Argument is P p && p.OperatorName == "eq"
                            ? p.Value
                            : step.Argument))
                    .Override<IdentityStep>((step, overridden, recurse) => CreateInstruction("identity"))
                    .Override<IdStep>((step, overridden, recurse) => CreateInstruction("id"))
                    .Override<IGremlinQueryBase>((query, overridden, recurse) => recurse.Serialize(query.ToTraversal()))
                    .Override<ILambda>((lambda, overridden, recurse) => lambda)
                    .Override<InjectStep>((step, overridden, recurse) => CreateInstruction("inject", recurse, step.Elements))
                    .Override<InEStep>((step, overridden, recurse) => CreateInstruction("inE", recurse, step.Labels))
                    .Override<InStep>((step, overridden, recurse) => CreateInstruction("in", recurse, step.Labels))
                    .Override<InVStep>((step, overridden, recurse) => CreateInstruction("inV"))
                    .Override<IsStep>((step, overridden, recurse) => CreateInstruction(
                        "is",
                        recurse,
                        step.Predicate.OperatorName == "eq"
                            ? step.Predicate.Value
                            : step.Predicate))
                    .Override<KeyStep>((step, overridden, recurse) => CreateInstruction("key"))
                    .Override<LabelStep>((step, overridden, recurse) => CreateInstruction("label"))
                    .Override<LimitStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("limit", recurse, step.Scope, step.Count)
                        : CreateInstruction("limit", recurse, step.Count))
                    .Override<LocalStep>((step, overridden, recurse) => CreateInstruction("local", recurse, step.Traversal))
                    .Override<MaxStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("max", recurse, step.Scope)
                        : CreateInstruction("max"))
                    .Override<MatchStep>((step, overridden, recurse) => CreateInstruction("match", recurse, step.Traversals))
                    .Override<MapStep>((step, overridden, recurse) => CreateInstruction("map", recurse, step.Traversal))
                    .Override<MeanStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("mean", recurse, step.Scope)
                        : CreateInstruction("mean"))
                    .Override<MinStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("min", recurse, step.Scope)
                        : CreateInstruction("min"))
                    .Override<NoneStep>((step, overridden, recurse) => recurse.Serialize(GremlinQueryEnvironment.NoneWorkaround))
                    .Override<NotStep>((step, overridden, recurse) => CreateInstruction("not", recurse, step.Traversal))
                    .Override<OptionalStep>((step, overridden, recurse) => CreateInstruction("optional", recurse, step.Traversal))
                    .Override<OptionTraversalStep>((step, overridden, recurse) => CreateInstruction("option", recurse, step.Guard ?? Pick.None, step.OptionTraversal))
                    .Override<OrderStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("order", recurse, step.Scope)
                        : CreateInstruction("order"))
                    .Override<OrderStep.ByLambdaStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Lambda))
                    .Override<OrderStep.ByMemberStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Key, step.Order))
                    .Override<OrderStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal, step.Order))
                    .Override<OrStep>((step, overridden, recurse) => CreateInstruction("or", recurse, step.Traversals))
                    .Override<OutStep>((step, overridden, recurse) => CreateInstruction("out", recurse, step.Labels))
                    .Override<OutEStep>((step, overridden, recurse) => CreateInstruction("outE", recurse, step.Labels))
                    .Override<OutVStep>((step, overridden, recurse) => CreateInstruction("outV"))
                    .Override<OtherVStep>((step, overridden, recurse) => CreateInstruction("otherV"))
                    .Override<P>((p, overridden, recurse) => new P(
                        p.OperatorName,
                        !(p.Value is string) && p.Value is IEnumerable enumerable
                            ? enumerable
                                .Cast<object>()
                                .Select(x => recurse.Serialize(x))
                                .ToArray()
                            : recurse.Serialize(p.Value),
                        p.Other is { } other
                            ? recurse.Serialize(other) as P
                            : null))
                    .Override<ProfileStep>((step, overridden, recurse) => CreateInstruction("profile"))
                    .Override<PropertiesStep>((step, overridden, recurse) => CreateInstruction("properties", recurse, step.Keys))
                    .Override<PropertyStep>((step, overridden, recurse) =>
                    {
                        if (T.Id.Equals(step.Key))
                        {
                            if (!Cardinality.Single.Equals(step.Cardinality ?? Cardinality.Single))
                                throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

                            return CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).ToArray());
                        }

                        return (step.Cardinality != null)
                             ? CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).Prepend(step.Cardinality).ToArray())
                             : CreateInstruction("property", recurse, step.MetaProperties.Prepend(step.Value).Prepend(step.Key).ToArray());
                    })
                    .Override<ProjectStep>((step, overridden, recurse) => CreateInstruction("project", recurse, step.Projections))
                    .Override<ProjectStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                    .Override<ProjectStep.ByKeyStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Key))
                    .Override<RangeStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("range", recurse, step.Scope, step.Lower, step.Upper)
                        : CreateInstruction("range", recurse, step.Lower, step.Upper))
                    .Override<RepeatStep>((step, overridden, recurse) => CreateInstruction("repeat", recurse, step.Traversal))
                    .Override<SelectStep>((step, overridden, recurse) => CreateInstruction("select", recurse, step.StepLabels))
                    .Override<SideEffectStep>((step, overridden, recurse) => CreateInstruction("sideEffect", recurse, step.Traversal))
                    .Override<SkipStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("skip", recurse, step.Scope, step.Count)
                        : CreateInstruction("skip", recurse, step.Count))
                    .Override<Step>((step, overridden, recurse) => Array.Empty<Instruction>())
                    .Override<Traversal>((traversal, overridden, recurse) =>
                    {
                        var byteCode = new Bytecode();
                        var steps = traversal.Steps;

                        void Add(object serialized)
                        {
                            switch (serialized)
                            {
                                case Instruction instruction:
                                {
                                    if (instruction.OperatorName.Equals("withoutStrategies"))
                                        byteCode.SourceInstructions.Add(instruction);
                                    else
                                        byteCode.StepInstructions.Add(instruction);

                                    break;
                                }
                                case Step step:
                                {
                                    Add(recurse.Serialize(step));

                                    break;
                                }
                                case IEnumerable enumerable:
                                {
                                    foreach (var item in enumerable)
                                    {
                                        Add(item);
                                    }

                                    break;
                                }
                            }
                        }

                        if (steps.Length == 0)
                            steps = IdentitySteps;

                        Add(steps);

                        return byteCode;
                    })
                    .Override<SumStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("sum", recurse, step.Scope)
                        : CreateInstruction("sum"))
                    .Override<TextP>((textP, overridden, recurse) => textP)
                    .Override<TailStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                        ? CreateInstruction("tail", recurse, step.Scope, step.Count)
                        : CreateInstruction("tail", recurse, step.Count))
                    .Override<TimesStep>((step, overridden, recurse) => CreateInstruction("times", recurse, step.Count))
                    .Override<Type>((type, overridden, recurse) => type)
                    .Override<UnfoldStep>((step, overridden, recurse) => CreateInstruction("unfold"))
                    .Override<UnionStep>((step, overridden, recurse) => CreateInstruction("union", recurse, step.Traversals))
                    .Override<UntilStep>((step, overridden, recurse) => CreateInstruction("until", recurse, step.Traversal))
                    .Override<ValueStep>((step, overridden, recurse) => CreateInstruction("value"))
                    .Override<ValueMapStep>((step, overridden, recurse) => CreateInstruction("valueMap", recurse, step.Keys))
                    .Override<ValuesStep>((step, overridden, recurse) => CreateInstruction("values", recurse, step.Keys))
                    .Override<VStep>((step, overridden, recurse) => CreateInstruction("V", recurse, step.Ids))
                    .Override<WhereTraversalStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Traversal))
                    .Override<WithStrategiesStep>((step, overridden, recurse) => CreateInstruction("withStrategies", recurse, step.Traversal))
                    .Override<WithoutStrategiesStep>((step, overridden, recurse) => CreateInstruction("withoutStrategies", recurse, step.StrategyTypes))
                    .Override<WherePredicateStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Predicate))
                    .Override<WherePredicateStep.ByMemberStep>((step, overridden, recurse) => step.Key != null
                        ? CreateInstruction("by", recurse, step.Key)
                        : CreateInstruction("by"))
                    .Override<WhereStepLabelAndPredicateStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.StepLabel, step.Predicate)));
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
                    return serialized switch
                    {
                        GroovyGremlinQuery serializedQuery => serializedQuery,
                        Bytecode bytecode => bytecode.ToGroovy(),
                        _ => throw new NotSupportedException($"Can't convert serialized query of type {serialized.GetType()} to {nameof(GroovyGremlinQuery)}.")
                    };
                });
        }

        private static Instruction CreateInstruction(string name)
        {
            return SimpleInstructions.GetOrAdd(
                name,
                closure => new Instruction(closure));
        }

        private static Instruction CreateInstruction<TParam>(string name, IQueryFragmentSerializer recurse, TParam parameter)
        {
            return new Instruction(
                name,
                recurse.Serialize(parameter));
        }

        private static Instruction CreateInstruction<TParam1, TParam2>(string name, IQueryFragmentSerializer recurse, TParam1 parameter1, TParam2 parameter2)
        {
            return new Instruction(
                name,
                recurse.Serialize(parameter1),
                recurse.Serialize(parameter2));
        }

        private static Instruction CreateInstruction<TParam1, TParam2, TParam3>(string name, IQueryFragmentSerializer recurse, TParam1 parameter1, TParam2 parameter2, TParam3 parameter3)
        {
            return new Instruction(
                name,
                recurse.Serialize(parameter1),
                recurse.Serialize(parameter2),
                recurse.Serialize(parameter3));
        }

        private static Instruction CreateInstruction<TParam>(string name, IQueryFragmentSerializer recurse, TParam[] parameters)
        {
            return parameters.Length == 0
                ? CreateInstruction(name)
                : CreateInstruction(name, recurse, (IEnumerable<TParam>)parameters);
        }

        private static Instruction CreateInstruction<TParam>(string name, IQueryFragmentSerializer recurse, ImmutableArray<TParam> parameters)
        {
            return parameters.Length == 0
                ? CreateInstruction(name)
                : CreateInstruction(name, recurse, (IEnumerable<TParam>)parameters);
        }

        private static Instruction CreateInstruction<TParam>(string name, IQueryFragmentSerializer recurse, IEnumerable<TParam> parameters)
        {
            return new Instruction(
                name,
                parameters
                    .Select(x => recurse.Serialize(x))
                    .ToArray());
        }
    }
}
