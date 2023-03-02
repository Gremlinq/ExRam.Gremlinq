using System.Buffers;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class Serializer
    {
        [ThreadStatic]
        internal static Dictionary<StepLabel, string>? _stepLabelNames;

        internal static readonly string[] StepLabelNameCache;

        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new();

        private sealed class SelectSerializer : ITransformer
        {
            private readonly Func<object, object> _projection;
            private readonly ITransformer _baseSerializer;

            public SelectSerializer(ITransformer baseSerializer, Func<object, object> projection)
            {
                _projection = projection;
                _baseSerializer = baseSerializer;
            }

            public ITransformer Add(IConverterFactory converterFactory) => new SelectSerializer(_baseSerializer.Add(converterFactory), _projection);

            public bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value)
            {
                if (_baseSerializer.TryTransform<TSource, TTarget>(source, environment, out var serialized))
                {
                    if (_projection(serialized) is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        private sealed class FixedTypeConverterFactory<TStaticSource> : IConverterFactory
        {
            private sealed class FixedTypeConverter<TSource> : IConverter<TSource, object>
            {
                private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, object?> _func;

                public FixedTypeConverter(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, object?> func)
                {
                    _func = func;
                }

                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out object? value)
                {
                    if (source is TStaticSource staticSerialized && _func(staticSerialized, environment, defer, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, object?> _func;

            public FixedTypeConverterFactory(Func<TStaticSource, IGremlinQueryEnvironment, ITransformer, ITransformer, object?> del)
            {
                _func = del;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return ((typeof(TSource).IsAssignableFrom(typeof(TStaticSource)) || typeof(TStaticSource).IsAssignableFrom(typeof(TSource))) && (typeof(TTarget) == typeof(object)))
                    ? (IConverter<TSource, TTarget>)(object)new FixedTypeConverter<TSource>(_func)
                    : null;
            }
        }

        public static readonly ITransformer Default = Transformer.Identity
            .UseDefaultGremlinStepSerializationHandlers();

        static Serializer()
        {
            StepLabelNameCache = Enumerable.Range(1, 100)
                .Select(static x => "l" + x)
                .ToArray();
        }

        public static ITransformer Select(this ITransformer serializer, Func<object, object> projection)
        {
            return new SelectSerializer(serializer, projection);
        }

        public static ITransformer ToGroovy(this ITransformer serializer)
        {
            return serializer
                .Select(static serialized => serialized is ISerializedGremlinQuery serializedQuery
                    ? serializedQuery.ToGroovy()
                    : serialized);
        }

        public static ITransformer Add<TSource>(this ITransformer serializer, Func<TSource, IGremlinQueryEnvironment, ITransformer, object?> converter)
        {
            return serializer
                .Add<TSource>((source, env, _, recurse) => converter(source, env, recurse));
        }

        public static ITransformer Add<TSource>(this ITransformer serializer, Func<TSource, IGremlinQueryEnvironment, ITransformer, ITransformer, object?> converter)
        {
            return serializer
                .Add(new FixedTypeConverterFactory<TSource>(converter));
        }

        public static ITransformer UseDefaultGremlinStepSerializationHandlers(this ITransformer serializer) => serializer
            .Add<AddEStep>(static (step, env, recurse) => CreateInstruction("addE", recurse, env, step.Label))
            .Add<AddEStep.ToLabelStep>(static (step, env, recurse) => CreateInstruction("to", recurse, env, step.StepLabel))
            .Add<AddEStep.ToTraversalStep>(static (step, env, recurse) => CreateInstruction("to", recurse, env, step.Traversal))
            .Add<AddEStep.FromLabelStep>(static (step, env, recurse) => CreateInstruction("from", recurse, env, step.StepLabel))
            .Add<AddEStep.FromTraversalStep>(static (step, env, recurse) => CreateInstruction("from", recurse, env, step.Traversal))
            .Add<AddVStep>(static (step, env, recurse) => CreateInstruction("addV", recurse, env, step.Label))
            .Add<AndStep>(static (step, env, recurse) => CreateInstruction("and", recurse, env, step.Traversals))
            .Add<AggregateStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Global)
                ? CreateInstruction("aggregate", recurse, env, step.StepLabel)
                : CreateInstruction("aggregate", recurse, env, step.Scope, step.StepLabel))
            .Add<AsStep>(static (step, env, recurse) => CreateInstruction("as", recurse, env, step.StepLabel))
            .Add<BarrierStep>(static (_, _, _) => CreateInstruction("barrier"))
            .Add<BothStep>(static (step, env, recurse) => CreateInstruction("both", recurse, env, step.Labels))
            .Add<BothEStep>(static (step, env, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
            .Add<BothVStep>(static (_, _, _) => CreateInstruction("bothV"))
            .Add<CapStep>(static (step, env, recurse) => CreateInstruction("cap", recurse, env, step.StepLabel))
            .Add<ChooseOptionTraversalStep>(static (step, env, recurse) => CreateInstruction("choose", recurse, env, step.Traversal))
            .Add<ChoosePredicateStep>(static (step, env, recurse) => step.ElseTraversal is { } elseTraversal
                ? CreateInstruction(
                    "choose",
                    recurse,
                    env,
                    step.Predicate,
                    step.ThenTraversal,
                    elseTraversal)
                : CreateInstruction(
                    "choose",
                    recurse,
                    env,
                    step.Predicate,
                    step.ThenTraversal))
            .Add<ChooseTraversalStep>(static (step, env, recurse) => step.ElseTraversal is { } elseTraversal
                ? CreateInstruction(
                    "choose",
                    recurse,
                    env,
                    step.IfTraversal,
                    step.ThenTraversal,
                    elseTraversal)
                : CreateInstruction(
                    "choose",
                    recurse,
                    env,
                    step.IfTraversal,
                    step.ThenTraversal))
            .Add<CoalesceStep>(static (step, env, recurse) => CreateInstruction("coalesce", recurse, env, step.Traversals))
            .Add<CoinStep>(static (step, env, recurse) => CreateInstruction("coin", recurse, env, step.Probability))
            .Add<ConstantStep>(static (step, env, recurse) => CreateInstruction("constant", recurse, env, step.Value))
            .Add<CountStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("count", recurse, env, step.Scope)
                : CreateInstruction("count"))
            .Add<CyclicPathStep>(static (_, _, _) => CreateInstruction("cyclicPath"))
            .Add<DateTime>(static (dateTime, env, recurse) => recurse.TransformTo<object>().From(new DateTimeOffset(dateTime.ToUniversalTime()), env))
            .Add<DedupStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("dedup", recurse, env, step.Scope)
                : CreateInstruction("dedup"))
            .Add<DropStep>(static (_, _, _) => CreateInstruction("drop"))
            .Add<EmitStep>(static (_, _, _) => CreateInstruction("emit"))
            .Add<EStep>(static (step, env, recurse) => CreateInstruction("E", recurse, env, step.Ids))
            .Add<ExplainStep>(static (_, _, _) => CreateInstruction("explain"))
            .Add<FailStep>(static (step, env, recurse) => step.Message is { } message
                ? CreateInstruction("fail", recurse, env, message)
                : CreateInstruction("fail"))
            .Add<FoldStep>(static (_, _, _) => CreateInstruction("fold"))
            .Add<FilterStep.ByLambdaStep>(static (step, env, recurse) => CreateInstruction("filter", recurse, env, step.Lambda))
            .Add<FilterStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("filter", recurse, env, step.Traversal))
            .Add<FlatMapStep>(static (step, env, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
            .Add<GroupStep>(static (_, _, _) => CreateInstruction("group"))
            .Add<GroupStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal))
            .Add<GroupStep.ByKeyStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Add<HasStep>(static (step, env, recurse) => CreateInstruction("has", recurse, env, step.Key))
            .Add<HasKeyStep>(static (step, env, recurse) => CreateInstruction(
                "hasKey",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq" } p
                    ? (object)p.Value
                    : step.Argument))
            .Add<HasPredicateStep>(static (step, env, recurse) => CreateInstruction(
                "has",
                recurse,
                env,
                step.Key,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Add<HasTraversalStep>(static (step, env, recurse) => CreateInstruction("has", recurse, env, step.Key, step.Traversal))
            .Add<HasLabelStep>(static (step, env, recurse) => CreateInstruction("hasLabel", recurse, env, step.Labels))
            .Add<HasNotStep>(static (step, env, recurse) => CreateInstruction("hasNot", recurse, env, step.Key))
            .Add<HasValueStep>(static (step, env, recurse) => CreateInstruction(
                "hasValue",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq" } p
                    ? (object)p.Value
                    : step.Argument))
            .Add<IdentityStep>(static (_, _, _) => CreateInstruction("identity"))
            .Add<IdStep>(static (_, _, _) => CreateInstruction("id"))
            .Add<IGremlinQueryBase>(static (query, env, recurse) =>
            {
                var serialized = recurse
                    .TransformTo<object>()
                    .From(
                        query
                            .ToTraversal()
                            .IncludeProjection(env),
                        env);

                return (serialized is Bytecode bytecode)
                    ? new BytecodeGremlinQuery(bytecode)
                    : serialized;
            })
            .Add<InjectStep>(static (step, env, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
            .Add<InEStep>(static (step, env, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
            .Add<InStep>(static (step, env, recurse) => CreateInstruction("in", recurse, env, step.Labels))
            .Add<InVStep>(static (_, _, _) => CreateInstruction("inV"))
            .Add<IsStep>(static (step, env, recurse) => CreateInstruction(
                "is",
                recurse,
                env,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Add<Key>(static (key, env, recurse) => recurse.TransformTo<object>().From(key.RawKey, env))
            .Add<KeyStep>(static (_, _, _) => CreateInstruction("key"))
            .Add<LabelStep>(static (_, _, _) => CreateInstruction("label"))
            .Add<LimitStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                : CreateInstruction("limit", recurse, env, step.Count))
            .Add<LocalStep>(static (step, env, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
            .Add<MaxStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("max", recurse, env, step.Scope)
                : CreateInstruction("max"))
            .Add<MatchStep>(static (step, env, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
            .Add<MapStep>(static (step, env, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
            .Add<MeanStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("mean", recurse, env, step.Scope)
                : CreateInstruction("mean"))
            .Add<Memory<Step>>(static (steps, env, recurse) =>
            {
                var j = 0;
                var span = steps.Span;

                for (var i = 1; i < span.Length; i++)
                {
                    var sourceStep = span[i];
                    var targetStep = span[i - j - 1];

                    if (sourceStep is SelectStepLabelStep selectStep && targetStep is AsStep asStep && selectStep.StepLabels.Length == 1 && ReferenceEquals(asStep.StepLabel, selectStep.StepLabels[0]))
                        j++;
                    else if (sourceStep is AsStep asStep1 && targetStep is AsStep asStep2 && asStep1.StepLabel.Equals(asStep2.StepLabel))
                        j++;
                    else if (sourceStep is IdentityStep && targetStep is IdentityStep)
                        j++;
                    else if (sourceStep is NoneStep && targetStep is NoneStep)
                        j++;
                    else if (sourceStep is HasLabelStep hasLabelStep1 && targetStep is HasLabelStep hasLabelStep2)
                    {
                        span[i - j - 1] = new HasLabelStep(hasLabelStep1.Labels.Intersect(hasLabelStep2.Labels).ToImmutableArray());
                        j++;
                    }
                    else if (sourceStep is HasPredicateStep hasPredicateStep1 && targetStep is HasPredicateStep hasPredicateStep2 && hasPredicateStep1.Key == hasPredicateStep2.Key)
                    {
                        span[i - j - 1] = new HasPredicateStep(hasPredicateStep1.Key, hasPredicateStep2.Predicate.And(hasPredicateStep1.Predicate));
                        j++;
                    }
                    else if (sourceStep is WithoutStrategiesStep withoutStrategiesStep1 && targetStep is WithoutStrategiesStep withoutStrategiesStep2)
                    {
                        span[i - j - 1] = new WithoutStrategiesStep(withoutStrategiesStep2.StrategyTypes.Concat(withoutStrategiesStep1.StrategyTypes).Distinct().ToImmutableArray());
                        j++;
                    }
                    else if (j != 0)
                        span[i - j] = sourceStep;
                }

                span = span[..^j];

                var byteCode = new Bytecode();

                void Add(object? obj)
                {
                    switch (obj)
                    {
                        case Instruction instruction:
                        {
                            if (byteCode.StepInstructions.Count == 0 && instruction.OperatorName.StartsWith("with", StringComparison.OrdinalIgnoreCase))
                                byteCode.SourceInstructions.Add(instruction);
                            else
                                byteCode.StepInstructions.Add(instruction);

                            break;
                        }
                        case Step step:
                        {
                            Add(recurse.TransformTo<object>().From(step, env));

                            break;
                        }
                        case Traversal traversal:
                        {
                            for (var i = 0; i < traversal.Count; i++)
                            {
                                Add(traversal[i]);
                            }

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

                foreach (var step in span)
                {
                    Add(step);
                }

                if (byteCode.StepInstructions.Count == 0)
                    Add(IdentityStep.Instance);

                return recurse.TransformTo<object>().From(byteCode, env);
            })
            .Add<MinStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("min", recurse, env, step.Scope)
                : CreateInstruction("min"))
            .Add<NoneStep>(static (_, _, _) => CreateInstruction("none"))
            .Add<NotStep>(static (step, env, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
            .Add<OptionalStep>(static (step, env, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
            .Add<OptionTraversalStep>(static (step, env, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
            .Add<OrderStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("order", recurse, env, step.Scope)
                : CreateInstruction("order"))
            .Add<OrderStep.ByLambdaStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Lambda))
            .Add<OrderStep.ByMemberStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
            .Add<OrderStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
            .Add<OrStep>(static (step, env, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
            .Add<OutStep>(static (step, env, recurse) => CreateInstruction("out", recurse, env, step.Labels))
            .Add<OutEStep>(static (step, env, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
            .Add<OutVStep>(static (_, _, _) => CreateInstruction("outV"))
            .Add<OtherVStep>(static (_, _, _) => CreateInstruction("otherV"))
            .Add<P>(static (p, env, recurse) =>
            {
                if (p.Value is null)
                    throw new NotSupportedException("Cannot serialize a P-predicate with a null-value.");

                return new P(
                    p.OperatorName,
                    p.Value is IEnumerable enumerable && !env.Model.NativeTypes.Contains(enumerable.GetType())
                        ? enumerable
                            .Cast<object>()
                            .Select(x => recurse.TransformTo<object>().From(x, env))
                            .ToArray()
                        : recurse.TransformTo<object>().From((object)p.Value, env),
                    p.Other is { } other
                        ? recurse.TransformTo<object>().From(other, env) as P
                        : null);
            })
            .Add<PathStep>(static (_, _, _) => CreateInstruction("path"))
            .Add<ProfileStep>(static (_, _, _) => CreateInstruction("profile"))
            .Add<PropertiesStep>(static (step, env, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
            .Add<PropertyStep.ByKeyStep>(static (step, env, recurse) =>
            {
                static object[] GetPropertyStepArguments(PropertyStep.ByKeyStep propertyStep, ITransformer recurse, IGremlinQueryEnvironment env)
                {
                    var i = 0;
                    object[] ret;

                    if (propertyStep.Cardinality != null && !T.Id.Equals(propertyStep.Key.RawKey))
                    {
                        ret = new object[propertyStep.MetaProperties.Length * 2 + 3];
                        ret[i++] = recurse.TransformTo<object>().From(propertyStep.Cardinality, env);
                    }
                    else
                        ret = new object[propertyStep.MetaProperties.Length * 2 + 2];

                    ret[i++] = recurse.TransformTo<object>().From(propertyStep.Key, env);
                    ret[i++] = recurse.TransformTo<object>().From(propertyStep.Value, env);

                    for (var j = 0; j < propertyStep.MetaProperties.Length; j++)
                    {
                        ret[i++] = recurse.TransformTo<object>().From(propertyStep.MetaProperties[j].Key, env);
                        ret[i++] = recurse.TransformTo<object>().From(propertyStep.MetaProperties[j].Value, env);
                    }

                    return ret;
                }

                return (T.Id.Equals(step.Key.RawKey) && !Cardinality.Single.Equals(step.Cardinality ?? Cardinality.Single))
                    ? throw new NotSupportedException("Cannot have an id property on non-single cardinality.")
                    : new Instruction("property", GetPropertyStepArguments(step, recurse, env));
            })
            .Add<ProjectStep>(static (step, env, recurse) => CreateInstruction("project", recurse, env, step.Projections))
            .Add<ProjectStep.ByTraversalStep>(static (step, env, recurse) =>
            {
                var traversal = step.Traversal;

                if (traversal.Count == 1 && traversal[0] is LocalStep localStep)
                    traversal = localStep.Traversal;

                return CreateInstruction("by", recurse, env, traversal);
            })
            .Add<ProjectStep.ByKeyStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Add<ProjectVertexStep>(static (_, env, _) => env.Options.GetValue(env.FeatureSet.Supports(VertexFeatures.MetaProperties)
                ? GremlinqOption.VertexProjectionSteps
                : GremlinqOption.VertexProjectionWithoutMetaPropertiesSteps))
            .Add<ProjectEdgeStep>(static (_, env, _) => env.Options.GetValue(GremlinqOption.EdgeProjectionSteps))
            .Add<RangeStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
            .Add<RepeatStep>(static (step, env, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
            .Add<SelectColumnStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.Column))
            .Add<SelectKeysStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.Keys))
            .Add<SelectStepLabelStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
            .Add<SideEffectStep>(static (step, env, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
            .Add<SimplePathStep>(static (_, _, _) => CreateInstruction("simplePath"))
            .Add<SkipStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                : CreateInstruction("skip", recurse, env, step.Count))
            .Add<StepLabel>(static (stepLabel, env, recurse) =>
            {
                var stepLabelNames = _stepLabelNames ??= new Dictionary<StepLabel, string>();

                if (!stepLabelNames!.TryGetValue(stepLabel, out var stepLabelMapping))
                {
                    stepLabelMapping = stepLabel.Identity as string ?? (stepLabelNames.Count < StepLabelNameCache.Length
                        ? StepLabelNameCache[stepLabelNames.Count]
                        : "l" + (stepLabelNames.Count + 1));

                    stepLabelNames.Add(stepLabel, stepLabelMapping);
                }

                // ReSharper disable once TailRecursiveCall
                return recurse.TransformTo<object>().From(stepLabelMapping, env);
            })
            .Add<Traversal>(static (traversal, env, recurse) =>
            {
                var steps = ArrayPool<Step>.Shared.Rent(traversal.Count);

                try
                {
                    var stepsMemory = steps
                        .AsMemory()[..traversal.Count];

                    traversal.Steps
                        .AsSpan()
                        .CopyTo(stepsMemory.Span);

                    return recurse.TransformTo<object>().From(stepsMemory, env);
                }
                finally
                {
                    ArrayPool<Step>.Shared.Return(steps);
                }
            })
            .Add<SumStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("sum", recurse, env, step.Scope)
                : CreateInstruction("sum"))
            .Add<TextP>(static (textP, _, _) => textP)
            .Add<TailStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                : CreateInstruction("tail", recurse, env, step.Count))
            .Add<TimesStep>(static (step, env, recurse) => CreateInstruction("times", recurse, env, step.Count))
            .Add<Type>(static (type, _, _) => type)
            .Add<UnfoldStep>(static (_, _, _) => CreateInstruction("unfold"))
            .Add<UnionStep>(static (step, env, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
            .Add<UntilStep>(static (step, env, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
            .Add<ValueStep>(static (_, _, _) => CreateInstruction("value"))
            .Add<ValueMapStep>(static (step, env, recurse) => CreateInstruction("valueMap", recurse, env, step.Keys))
            .Add<ValuesStep>(static (step, env, recurse) => CreateInstruction("values", recurse, env, step.Keys))
            .Add<VStep>(static (step, env, recurse) => CreateInstruction("V", recurse, env, step.Ids))
            .Add<WhereTraversalStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.Traversal))
            .Add<WithStrategiesStep>(static (step, env, recurse) => CreateInstruction("withStrategies", recurse, env, step.Traversal))
            .Add<WithoutStrategiesStep>(static (step, env, recurse) => CreateInstruction("withoutStrategies", recurse, env, step.StrategyTypes))
            .Add<WithSideEffectStep>(static (step, env, recurse) => CreateInstruction("withSideEffect", recurse, env, step.Label, step.Value))
            .Add<WherePredicateStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.Predicate))
            .Add<WherePredicateStep.ByMemberStep>(static (step, env, recurse) => step.Key is { } key
                ? CreateInstruction("by", recurse, env, key)
                : CreateInstruction("by"))
            .Add<WhereStepLabelAndPredicateStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate));

        internal static ISerializedGremlinQuery Serialize(this ITransformer serializer, IGremlinQueryBase query)
        {
            try
            {
                _stepLabelNames = null;

                var serialized = serializer
                    .TransformTo<object>().From(query, query.AsAdmin().Environment);

                if (serialized is ISerializedGremlinQuery serializedQuery)
                    return serializedQuery;

                throw new InvalidOperationException($"Unable to serialize a query of type {query.GetType().FullName}.");
            }
            finally
            {
                _stepLabelNames = null;
            }
        }

        private static Instruction CreateInstruction(string name)
        {
            return SimpleInstructions.GetOrAdd(
                name,
                static closure => new Instruction(closure));
        }

        private static Instruction CreateInstruction<TParam>(string name, ITransformer recurse, IGremlinQueryEnvironment env, TParam parameter)
        {
            return new(
                name,
                recurse.NullAwareSerialize(parameter, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2>(string name, ITransformer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2)
        {
            return new(
                name,
                recurse.NullAwareSerialize(parameter1, env),
                recurse.NullAwareSerialize(parameter2, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2, TParam3>(string name, ITransformer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2, TParam3 parameter3)
        {
            return new(
                name,
                recurse.NullAwareSerialize(parameter1, env),
                recurse.NullAwareSerialize(parameter2, env),
                recurse.NullAwareSerialize(parameter3, env));
        }

        private static Instruction CreateInstruction<TParam>(string name, ITransformer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters)
        {
            if (parameters.Length == 0)
                return CreateInstruction(name);

            var arguments = new object?[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                arguments[i] = recurse.NullAwareSerialize(parameters[i], env);
            }

            return new Instruction(name, arguments);
        }

        private static object? NullAwareSerialize<TParam>(this ITransformer serializer, TParam maybeParameter, IGremlinQueryEnvironment env)
        {
            return maybeParameter is { } parameter
                ? serializer.TransformTo<object>().From(parameter, env)
                : default;
        }
    }
}

