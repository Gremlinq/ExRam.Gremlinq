using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer
    {
        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            private readonly IImmutableDictionary<Type, QueryFragmentSerializer<object>> _dict;
            private ConcurrentDictionary<Type, QueryFragmentSerializer<object>?>? _fastDict;

            public GremlinQuerySerializerImpl(IImmutableDictionary<Type, QueryFragmentSerializer<object>> dict)
            {
                _dict = dict;
            }

            public object? Serialize(IGremlinQueryBase query)
            {
                var stepLabelNames = new Dictionary<StepLabel, string>();

                object Constant<TAtom>(TAtom atom, Func<TAtom, object> baseSerializer, Func<object, object?> recurse)
                {
                    if (atom is StepLabel stepLabel)
                    {
                        if (!stepLabelNames.TryGetValue(stepLabel, out var stepLabelMapping))
                        {
                            stepLabelMapping = "l" + (stepLabelNames.Count + 1);
                            stepLabelNames.Add(stepLabel, stepLabelMapping);
                        }

                        // ReSharper disable once TailRecursiveCall
                        return recurse(stepLabelMapping);
                    }

                    return atom!;
                }

                object? RecurseImpl(object? o)
                {
                    if (o == null)
                        return null;

                    var action = TryGetSerializer(o.GetType()) ?? Constant;

                    return action(o, _ => Constant(_, _ => _, RecurseImpl), RecurseImpl);
                }

                return RecurseImpl(query);
            }

            public IGremlinQuerySerializer OverrideFragmentSerializer<TAtom>(QueryFragmentSerializer<TAtom> queryFragmentSerializer)
            {
                return new GremlinQuerySerializerImpl(_dict.SetItem(
                    typeof(TAtom),
                    _dict.TryGetValue(typeof(TAtom), out var existingAtomSerializer)
                        ? new QueryFragmentSerializer<object>((atom, baseSerializer, recurse) => queryFragmentSerializer((TAtom)atom, _ => existingAtomSerializer(_!, baseSerializer, recurse), recurse))
                        : (atom, baseSerializer, recurse) => queryFragmentSerializer((TAtom)atom, _ => baseSerializer(_!), recurse)));
            }

            private QueryFragmentSerializer<object>? TryGetSerializer(Type type)
            {
                var fastDict = Volatile.Read(ref _fastDict);
                if (fastDict == null)
                    Volatile.Write(ref _fastDict, fastDict = new ConcurrentDictionary<Type, QueryFragmentSerializer<object>?>(_dict));

                return fastDict
                    .GetOrAdd(
                        type,
                        (closureType, @this) =>
                        {
                            foreach (var implementedInterface in closureType.GetInterfaces())
                            {
                                if (@this.TryGetSerializer(implementedInterface) is { } interfaceSerializer)
                                    return interfaceSerializer;
                            }

                            if (closureType.BaseType is { } baseType)
                            {
                                if (@this.TryGetSerializer(baseType) is { } baseSerializer)
                                    return baseSerializer;
                            }

                            return null;
                        },
                        this);
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public IGremlinQuerySerializer OverrideFragmentSerializer<TAtom>(QueryFragmentSerializer<TAtom> queryFragmentSerializer) => throw new InvalidOperationException($"{nameof(OverrideFragmentSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");

            public object Serialize(IGremlinQueryBase query) => throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
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

            public IGremlinQuerySerializer OverrideFragmentSerializer<TAtom>(QueryFragmentSerializer<TAtom> queryFragmentSerializer) => new SelectGremlinQuerySerializer(_baseSerializer.OverrideFragmentSerializer(queryFragmentSerializer), _projection);

            public object? Serialize(IGremlinQueryBase query)
            {
                return (_baseSerializer.Serialize(query) is { } serialized)
                    ? _projection(serialized)
                    : null;
            }
        }

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Identity = new GremlinQuerySerializerImpl(ImmutableDictionary<Type, QueryFragmentSerializer<object>>.Empty);

        public static readonly IGremlinQuerySerializer Default = Identity
            .UseDefaultGremlinStepSerializationHandlers();

        private static readonly Instruction[] VertexProjectionInstructions;
        private static readonly Instruction[] EdgeProjectionInstructions;
        private static readonly Instruction[] VertexProjectionInstructionsWithoutMetaProperties;
        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new ConcurrentDictionary<string, Instruction>();

        static GremlinQuerySerializer()
        {
            //CosmosDB workaround
            var labelBytecode = new Bytecode
            {
                StepInstructions =
                {
                    new Instruction("label")
                }
            };

            //CosmosDB workaround
            var valueBytecode = new Bytecode
            {
                StepInstructions =
                {
                    new Instruction("value")
                }
            };

            VertexProjectionInstructions = new[]
            {
                new Instruction("project", "id", "label", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
                new Instruction(
                    "by",
                    new Bytecode
                    {
                        StepInstructions =
                        {
                            new Instruction("properties"),
                            new Instruction("group"),
                            new Instruction("by", labelBytecode),
                            new Instruction("by", new Bytecode
                            {
                                StepInstructions =
                                {
                                    new Instruction("project", "id", "label", "value", "properties"),
                                    new Instruction("by", T.Id),
                                    new Instruction("by", labelBytecode),
                                    new Instruction("by", valueBytecode),
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

            VertexProjectionInstructionsWithoutMetaProperties = new[]
            {
                new Instruction("project", "id", "label", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
                new Instruction(
                    "by",
                    new Bytecode
                    {
                        StepInstructions =
                        {
                            new Instruction("properties"),
                            new Instruction("group"),
                            new Instruction("by", labelBytecode),
                            new Instruction("by", new Bytecode
                            {
                                StepInstructions =
                                {
                                    new Instruction("project", "id", "label", "value"),
                                    new Instruction("by", T.Id),
                                    new Instruction("by", labelBytecode),
                                    new Instruction("by", valueBytecode),
                                    new Instruction("fold")
                                }
                            })
                        }
                    })
            };

            EdgeProjectionInstructions = new[]
            {
                new Instruction("project", "id", "label", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
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
                .OverrideFragmentSerializer<AndStep>((step, overridden, recurse) => CreateInstruction("and", recurse, step.Traversals))
                .OverrideFragmentSerializer<AggregateStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("aggregate", recurse, step.StepLabel)
                    : CreateInstruction("aggregate", recurse, step.Scope, step.StepLabel))
                .OverrideFragmentSerializer<AsStep>((step, overridden, recurse) => CreateInstruction("as", recurse, step.StepLabels))
                .OverrideFragmentSerializer<BarrierStep>((step, overridden, recurse) => CreateInstruction("barrier"))
                .OverrideFragmentSerializer<BothStep>((step, overridden, recurse) => CreateInstruction("both", recurse, step.Labels))
                .OverrideFragmentSerializer<BothEStep>((step, overridden, recurse) => CreateInstruction("bothE", recurse, step.Labels))
                .OverrideFragmentSerializer<BothVStep>((step, overridden, recurse) => CreateInstruction("bothV"))
                .OverrideFragmentSerializer<CapStep>((step, overridden, recurse) => CreateInstruction("cap", recurse, step.StepLabel))
                .OverrideFragmentSerializer<ChooseOptionTraversalStep>((step, overridden, recurse) => CreateInstruction("choose", recurse, step.Traversal))
                .OverrideFragmentSerializer<ChoosePredicateStep>((step, overridden, recurse) =>
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
                .OverrideFragmentSerializer<ChooseTraversalStep>((step, overridden, recurse) =>
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
                .OverrideFragmentSerializer<CoalesceStep>((step, overridden, recurse) => CreateInstruction("coalesce", recurse, step.Traversals.ToArray()))
                .OverrideFragmentSerializer<CoinStep>((step, overridden, recurse) => CreateInstruction("coin", recurse, step.Probability))
                .OverrideFragmentSerializer<ConstantStep>((step, overridden, recurse) => CreateInstruction("constant", recurse, step.Value))
                .OverrideFragmentSerializer<CountStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("count", recurse, step.Scope)
                    : CreateInstruction("count"))
                .OverrideFragmentSerializer<DedupStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("dedup", recurse, step.Scope)
                    : CreateInstruction("dedup"))
                .OverrideFragmentSerializer<DropStep>((step, overridden, recurse) => CreateInstruction("drop"))
                .OverrideFragmentSerializer<EmitStep>((step, overridden, recurse) => CreateInstruction("emit"))
                .OverrideFragmentSerializer<EnumWrapper>((enumValue, overridden, recurse) => enumValue)
                .OverrideFragmentSerializer<EStep>((step, overridden, recurse) => CreateInstruction("E", recurse, step.Ids))
                .OverrideFragmentSerializer<Expression>((expression, overridden, recurse) => recurse(expression.GetValue()))
                .OverrideFragmentSerializer<ExplainStep>((step, overridden, recurse) => CreateInstruction("explain"))
                .OverrideFragmentSerializer<FoldStep>((step, overridden, recurse) => CreateInstruction("fold"))
                .OverrideFragmentSerializer<FilterStep>((step, overridden, recurse) => CreateInstruction("filter", recurse, step.Lambda))
                .OverrideFragmentSerializer<FlatMapStep>((step, overridden, recurse) => CreateInstruction("flatMap", recurse, step.Traversal))
                .OverrideFragmentSerializer<FromLabelStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.StepLabel))
                .OverrideFragmentSerializer<FromTraversalStep>((step, overridden, recurse) => CreateInstruction("from", recurse, step.Traversal))
                .OverrideFragmentSerializer<GroupStep>((step, overridden, recurse) => CreateInstruction("group"))
                .OverrideFragmentSerializer<GroupStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                .OverrideFragmentSerializer<HasKeyStep>((step, overridden, recurse) => CreateInstruction(
                    "hasKey",
                    recurse,
                    step.Argument is P p && p.OperatorName == "eq"
                        ? p.Value
                        : step.Argument))
                .OverrideFragmentSerializer<HasPredicateStep>((step, overridden, recurse) =>
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
                .OverrideFragmentSerializer<HasTraversalStep>((step, overridden, recurse) => CreateInstruction("has", recurse, step.Key, step.Traversal))
                .OverrideFragmentSerializer<HasLabelStep>((step, overridden, recurse) => CreateInstruction("hasLabel", recurse, step.Labels))
                .OverrideFragmentSerializer<HasNotStep>((step, overridden, recurse) => CreateInstruction("hasNot", recurse, step.Key))
                .OverrideFragmentSerializer<HasValueStep>((step, overridden, recurse) => CreateInstruction(
                    "hasValue",
                    recurse,
                    step.Argument is P p && p.OperatorName == "eq"
                        ? p.Value
                        : step.Argument))
                .OverrideFragmentSerializer<IdentityStep>((step, overridden, recurse) => CreateInstruction("identity"))
                .OverrideFragmentSerializer<IdStep>((step, overridden, recurse) => CreateInstruction("id"))
                .OverrideFragmentSerializer<IGremlinQueryBase>((query, overridden, recurse) =>
                {
                    var byteCode = new Bytecode();
                    var steps = query.AsAdmin().Steps;

                    if (steps.IsEmpty)
                    {
                        if (recurse(IdentityStep.Instance) is Instruction instruction)
                            byteCode.StepInstructions.Add(instruction);
                    }
                    else
                    {
                        var stepsArray = steps.ToArray();

                        for (var i = stepsArray.Length - 1; i >= 0; i--)
                        {
                            if (recurse(stepsArray[i]) is Instruction instruction)
                            {
                                if (instruction.OperatorName.Equals("withoutStrategies"))
                                    byteCode.SourceInstructions.Add(instruction);
                                else
                                    byteCode.StepInstructions.Add(instruction);
                            }
                        }
                    }

                    if (query is GremlinQueryBase gremlinQueryBase)
                    {
                        if (gremlinQueryBase.SurfaceVisible && !gremlinQueryBase.Environment.Options.GetValue(GremlinqOption.DontAddElementProjectionSteps))
                        {
                            switch (gremlinQueryBase.Semantics)
                            {
                                case QuerySemantics.Vertex:
                                {
                                    byteCode.StepInstructions.AddRange(gremlinQueryBase.Environment.FeatureSet.Supports(VertexFeatures.MetaProperties)
                                        ? VertexProjectionInstructions
                                        : VertexProjectionInstructionsWithoutMetaProperties);

                                    break;
                                }
                                case QuerySemantics.Edge:
                                {
                                    byteCode.StepInstructions.AddRange(EdgeProjectionInstructions);

                                    break;
                                }
                            }
                        }
                    }

                    return byteCode;
                })
                .OverrideFragmentSerializer<ILambda>((lambda, overridden, recurse) => lambda)
                .OverrideFragmentSerializer<InjectStep>((step, overridden, recurse) => CreateInstruction("inject", recurse, step.Elements))
                .OverrideFragmentSerializer<InEStep>((step, overridden, recurse) => CreateInstruction("inE", recurse, step.Labels))
                .OverrideFragmentSerializer<InStep>((step, overridden, recurse) => CreateInstruction("in", recurse, step.Labels))
                .OverrideFragmentSerializer<InVStep>((step, overridden, recurse) => CreateInstruction("inV"))
                .OverrideFragmentSerializer<IsStep>((step, overridden, recurse) => CreateInstruction(
                    "is",
                    recurse,
                    step.Predicate.OperatorName == "eq"
                        ? step.Predicate.Value
                        : step.Predicate))
                .OverrideFragmentSerializer<KeyStep>((step, overridden, recurse) => CreateInstruction("key"))
                .OverrideFragmentSerializer<LabelStep>((step, overridden, recurse) => CreateInstruction("label"))
                .OverrideFragmentSerializer<LimitStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("limit", recurse, step.Scope, step.Count)
                    : CreateInstruction("limit", recurse, step.Count))
                .OverrideFragmentSerializer<LocalStep>((step, overridden, recurse) => CreateInstruction("local", recurse, step.Traversal))
                .OverrideFragmentSerializer<MaxStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("max", recurse, step.Scope)
                    : CreateInstruction("max"))
                .OverrideFragmentSerializer<MatchStep>((step, overridden, recurse) => CreateInstruction("match", recurse, step.Traversals.ToArray()))
                .OverrideFragmentSerializer<MapStep>((step, overridden, recurse) => CreateInstruction("map", recurse, step.Traversal))
                .OverrideFragmentSerializer<MeanStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("mean", recurse, step.Scope)
                    : CreateInstruction("mean"))
                .OverrideFragmentSerializer<MinStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("min", recurse, step.Scope)
                    : CreateInstruction("min"))
                .OverrideFragmentSerializer<NoneStep>((step, overridden, recurse) => recurse(GremlinQueryEnvironment.NoneWorkaround))
                .OverrideFragmentSerializer<NotStep>((step, overridden, recurse) => CreateInstruction("not", recurse, step.Traversal))
                .OverrideFragmentSerializer<OptionalStep>((step, overridden, recurse) => CreateInstruction("optional", recurse, step.Traversal))
                .OverrideFragmentSerializer<OptionTraversalStep>((step, overridden, recurse) => CreateInstruction("option", recurse, step.Guard ?? Pick.None, step.OptionTraversal))
                .OverrideFragmentSerializer<OrderStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("order", recurse, step.Scope)
                    : CreateInstruction("order"))
                .OverrideFragmentSerializer<OrderStep.ByLambdaStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Lambda))
                .OverrideFragmentSerializer<OrderStep.ByMemberStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Key, step.Order))
                .OverrideFragmentSerializer<OrderStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal, step.Order))
                .OverrideFragmentSerializer<OrStep>((step, overridden, recurse) => CreateInstruction("or", recurse, step.Traversals))
                .OverrideFragmentSerializer<OutStep>((step, overridden, recurse) => CreateInstruction("out", recurse, step.Labels))
                .OverrideFragmentSerializer<OutEStep>((step, overridden, recurse) => CreateInstruction("outE", recurse, step.Labels))
                .OverrideFragmentSerializer<OutVStep>((step, overridden, recurse) => CreateInstruction("outV"))
                .OverrideFragmentSerializer<OtherVStep>((step, overridden, recurse) => CreateInstruction("otherV"))
                .OverrideFragmentSerializer<P>((p, overridden, recurse) => new P(
                    p.OperatorName,
                    !(p.Value is string) && p.Value is IEnumerable enumerable
                        ? enumerable
                            .Cast<object>()
                            .Select(recurse)
                            .ToArray()
                        : recurse(p.Value),
                    p.Other is { } other
                        ? recurse(other) as P
                        : null))
                .OverrideFragmentSerializer<ProfileStep>((step, overridden, recurse) => CreateInstruction("profile"))
                .OverrideFragmentSerializer<PropertiesStep>((step, overridden, recurse) => CreateInstruction("properties", recurse, step.Keys))
                .OverrideFragmentSerializer<PropertyStep>((step, overridden, recurse) =>
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
                .OverrideFragmentSerializer<ProjectStep.ByTraversalStep>((step, overridden, recurse) => CreateInstruction("by", recurse, step.Traversal))
                .OverrideFragmentSerializer<ProjectStep>((step, overridden, recurse) => CreateInstruction("project", recurse, step.Projections))
                .OverrideFragmentSerializer<RangeStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("range", recurse, step.Scope, step.Lower, step.Upper)
                    : CreateInstruction("range", recurse, step.Lower, step.Upper))
                .OverrideFragmentSerializer<RepeatStep>((step, overridden, recurse) => CreateInstruction("repeat", recurse, step.Traversal))
                .OverrideFragmentSerializer<SelectStep>((step, overridden, recurse) => CreateInstruction("select", recurse, step.StepLabels))
                .OverrideFragmentSerializer<SideEffectStep>((step, overridden, recurse) => CreateInstruction("sideEffect", recurse, step.Traversal))
                .OverrideFragmentSerializer<SkipStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("skip", recurse, step.Scope, step.Count)
                    : CreateInstruction("skip", recurse, step.Count))
                .OverrideFragmentSerializer<SumStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("sum", recurse, step.Scope)
                    : CreateInstruction("sum"))
                .OverrideFragmentSerializer<TextP>((textP, overridden, recurse) => textP)
                .OverrideFragmentSerializer<TailStep>((step, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("tail", recurse, step.Scope, step.Count)
                    : CreateInstruction("tail", recurse, step.Count))
                .OverrideFragmentSerializer<TimesStep>((step, overridden, recurse) => CreateInstruction("times", recurse, step.Count))
                .OverrideFragmentSerializer<Type>((type, overridden, recurse) => type)
                .OverrideFragmentSerializer<UnfoldStep>((step, overridden, recurse) => CreateInstruction("unfold"))
                .OverrideFragmentSerializer<UnionStep>((step, overridden, recurse) => CreateInstruction("union", recurse, step.Traversals.ToArray()))
                .OverrideFragmentSerializer<UntilStep>((step, overridden, recurse) => CreateInstruction("until", recurse, step.Traversal))
                .OverrideFragmentSerializer<ValueStep>((step, overridden, recurse) => CreateInstruction("value"))
                .OverrideFragmentSerializer<ValueMapStep>((step, overridden, recurse) => CreateInstruction("valueMap", recurse, step.Keys))
                .OverrideFragmentSerializer<ValuesStep>((step, overridden, recurse) => CreateInstruction("values", recurse, step.Keys))
                .OverrideFragmentSerializer<VStep>((step, overridden, recurse) => CreateInstruction("V", recurse, step.Ids))
                .OverrideFragmentSerializer<WhereTraversalStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Traversal))
                .OverrideFragmentSerializer<WithStrategiesStep>((step, overridden, recurse) => CreateInstruction("withStrategies", recurse, step.Traversal))
                .OverrideFragmentSerializer<WithoutStrategiesStep>((step, overridden, recurse) => CreateInstruction("withoutStrategies", recurse, step.StrategyTypes))
                .OverrideFragmentSerializer<WherePredicateStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.Predicate))
                .OverrideFragmentSerializer<WherePredicateStep.ByMemberStep>((step, overridden, recurse) => step.Key != null
                    ? CreateInstruction("by", recurse, step.Key)
                    : CreateInstruction("by", recurse))
                .OverrideFragmentSerializer<WhereStepLabelAndPredicateStep>((step, overridden, recurse) => CreateInstruction("where", recurse, step.StepLabel, step.Predicate));
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

        private static Instruction CreateInstruction(string name, Func<object, object> recurse, object parameter)
        {
            return new Instruction(name, recurse(parameter));
        }

        private static Instruction CreateInstruction(string name, Func<object, object> recurse, object parameter1, object parameter2)
        {
            return new Instruction(name, recurse(parameter1), recurse(parameter2));
        }

        private static Instruction CreateInstruction(string name, Func<object, object> recurse, params object[] parameters)
        {
            return parameters.Length == 0
                ? CreateInstruction(name)
                : new Instruction(
                    name,
                    parameters
                        .Select(recurse)
                        .ToArray());
        }
    }
}
