using System.Buffers;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;

using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class GremlinQuerySerializer
    {
        [ThreadStatic]
        internal static Dictionary<StepLabel, string>? _stepLabelNames;

        internal static readonly string[] StepLabelNameCache;

        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new();

        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            private static readonly MethodInfo CreateFuncMethod1 = typeof(GremlinQuerySerializerImpl).GetMethod(nameof(CreateFunc1), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod2 = typeof(GremlinQuerySerializerImpl).GetMethod(nameof(CreateFunc2), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod3 = typeof(GremlinQuerySerializerImpl).GetMethod(nameof(CreateFunc3), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod4 = typeof(GremlinQuerySerializerImpl).GetMethod(nameof(CreateFunc4), BindingFlags.NonPublic | BindingFlags.Static)!;

            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new();

            public GremlinQuerySerializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment)
            {
                var maybeRet = TryGetSerializer(typeof(TFragment), fragment!.GetType()) is BaseGremlinQueryFragmentSerializerDelegate<TFragment> del
                    ? del(fragment, gremlinQueryEnvironment, this)
                    : fragment;

                return maybeRet is { } ret
                    ? ret
                    : throw new InvalidOperationException();
            }

            public IGremlinQuerySerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer)
            {
                return new GremlinQuerySerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        TryGetSerializer(typeof(TFragment), typeof(TFragment)) is BaseGremlinQueryFragmentSerializerDelegate<TFragment> existingFragmentSerializer
                            ? (fragment, env, recurse) => serializer(fragment, env, recurse) is { } ret
                                ? ret
                                : existingFragmentSerializer(fragment, env, recurse)
                            : serializer));
            }

            private Delegate? TryGetSerializer(Type staticType, Type actualType)
            {
                return _fastDict
                    .GetOrAdd(
                        (staticType, actualType),
                        static (typeTuple, @this) =>
                        {
                            var (staticType, actualType) = typeTuple;

                            if (@this.InnerLookup(actualType) is { } del)
                            {
                                var effectiveType = del
                                    .GetType()
                                    .GetGenericArguments()[0];

                                var method = effectiveType == staticType
                                    ? CreateFuncMethod1.MakeGenericMethod(staticType)
                                    : staticType.IsConstructedGenericType && staticType.GetGenericTypeDefinition() == typeof(Nullable<>) && staticType.GetGenericArguments()[0] == effectiveType
                                        ? CreateFuncMethod2.MakeGenericMethod(effectiveType)
                                        : staticType.IsAssignableFrom(effectiveType)
                                            ? CreateFuncMethod3.MakeGenericMethod(staticType, effectiveType)
                                            : effectiveType.IsAssignableFrom(staticType)
                                                ? CreateFuncMethod4.MakeGenericMethod(staticType, effectiveType)
                                                : null;

                                return (Delegate)method?
                                    .Invoke(null, new object[] { del })!;
                            }

                            return null;
                        },
                        this);
            }

            private Delegate? InnerLookup(Type actualType)
            {
                if (_dict.TryGetValue(actualType, out var ret))
                    return ret;

                var baseType = actualType.BaseType;

                foreach (var implementedInterface in actualType.GetInterfaces())
                {
                    if ((baseType == null || !implementedInterface.IsAssignableFrom(baseType)) && InnerLookup(implementedInterface) is { } interfaceSerializer)
                        return interfaceSerializer;
                }

                return baseType != null && InnerLookup(baseType) is { } baseSerializer
                    ? baseSerializer
                    : null;
            }

            private static BaseGremlinQueryFragmentSerializerDelegate<TStatic> CreateFunc1<TStatic>(GremlinQueryFragmentSerializerDelegate<TStatic> del) => (fragment, environment, recurse) => del(fragment!, environment, recurse);

            private static BaseGremlinQueryFragmentSerializerDelegate<TEffective?> CreateFunc2<TEffective>(GremlinQueryFragmentSerializerDelegate<TEffective> del)
                where TEffective : struct => (fragment, environment, recurse) => fragment is { } value
                    ? del(value, environment, recurse)
                    : throw new ArgumentNullException();

            private static BaseGremlinQueryFragmentSerializerDelegate<TStatic> CreateFunc3<TStatic, TEffective>(GremlinQueryFragmentSerializerDelegate<TEffective> del)
                where TEffective : TStatic => (fragment, environment, recurse) => del((TEffective)(fragment ?? throw new ArgumentNullException()), environment, recurse);

            private static BaseGremlinQueryFragmentSerializerDelegate<TStatic> CreateFunc4<TStatic, TEffective>(GremlinQueryFragmentSerializerDelegate<TEffective> del)
                where TStatic : TEffective => (fragment, environment, recurse) => del(fragment, environment, recurse);
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public IGremlinQuerySerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer) => throw new InvalidOperationException($"{nameof(Override)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");

            public object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment) => throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
        }

        private sealed class SelectGremlinQuerySerializer : IGremlinQuerySerializer
        {
            private readonly Func<object, object> _projection;
            private readonly IGremlinQuerySerializer _baseSerializer;

            public SelectGremlinQuerySerializer(IGremlinQuerySerializer baseSerializer, Func<object, object> projection)
            {
                _projection = projection;
                _baseSerializer = baseSerializer;
            }

            public IGremlinQuerySerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer) => new SelectGremlinQuerySerializer(_baseSerializer.Override(serializer), _projection);

            public object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment) => _projection(_baseSerializer.Serialize(fragment, gremlinQueryEnvironment));
        }

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Identity = new GremlinQuerySerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);

        public static readonly IGremlinQuerySerializer Default = Identity.UseDefaultGremlinStepSerializationHandlers();

        static GremlinQuerySerializer()
        {
            StepLabelNameCache = Enumerable.Range(1, 100)
                .Select(static x => "l" + x)
                .ToArray();
        }

        public static IGremlinQuerySerializer Select(this IGremlinQuerySerializer serializer, Func<object, object> projection)
        {
            return new SelectGremlinQuerySerializer(serializer, projection);
        }

        public static IGremlinQuerySerializer ToGroovy(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .Select(static serialized => serialized is ISerializedGremlinQuery serializedQuery
                    ? serializedQuery.ToGroovy()
                    : serialized);
        }

        public static IGremlinQuerySerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQuerySerializer serializer) => serializer
            .Override<AddEStep>(static (step, env, recurse) => CreateInstruction("addE", recurse, env, step.Label))
            .Override<AddEStep.ToLabelStep>(static (step, env, recurse) => CreateInstruction("to", recurse, env, step.StepLabel))
            .Override<AddEStep.ToTraversalStep>(static (step, env, recurse) => CreateInstruction("to", recurse, env, step.Traversal))
            .Override<AddEStep.FromLabelStep>(static (step, env, recurse) => CreateInstruction("from", recurse, env, step.StepLabel))
            .Override<AddEStep.FromTraversalStep>(static (step, env, recurse) => CreateInstruction("from", recurse, env, step.Traversal))
            .Override<AddVStep>(static (step, env, recurse) => CreateInstruction("addV", recurse, env, step.Label))
            .Override<AndStep>(static (step, env, recurse) => CreateInstruction("and", recurse, env, step.Traversals))
            .Override<AggregateStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Global)
                ? CreateInstruction("aggregate", recurse, env, step.StepLabel)
                : CreateInstruction("aggregate", recurse, env, step.Scope, step.StepLabel))
            .Override<AsStep>(static (step, env, recurse) => CreateInstruction("as", recurse, env, step.StepLabel))
            .Override<BarrierStep>(static (_, _, _) => CreateInstruction("barrier"))
            .Override<BothStep>(static (step, env, recurse) => CreateInstruction("both", recurse, env, step.Labels))
            .Override<BothEStep>(static (step, env, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
            .Override<BothVStep>(static (_, _, _) => CreateInstruction("bothV"))
            .Override<CapStep>(static (step, env, recurse) => CreateInstruction("cap", recurse, env, step.StepLabel))
            .Override<ChooseOptionTraversalStep>(static (step, env, recurse) => CreateInstruction("choose", recurse, env, step.Traversal))
            .Override<ChoosePredicateStep>(static (step, env, recurse) => step.ElseTraversal is { } elseTraversal
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
            .Override<ChooseTraversalStep>(static (step, env, recurse) => step.ElseTraversal is { } elseTraversal
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
            .Override<CoalesceStep>(static (step, env, recurse) => CreateInstruction("coalesce", recurse, env, step.Traversals))
            .Override<CoinStep>(static (step, env, recurse) => CreateInstruction("coin", recurse, env, step.Probability))
            .Override<ConstantStep>(static (step, env, recurse) => CreateInstruction("constant", recurse, env, step.Value))
            .Override<CountStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("count", recurse, env, step.Scope)
                : CreateInstruction("count"))
            .Override<CyclicPathStep>(static (_, _, _) => CreateInstruction("cyclicPath"))
            .Override<DateTime>(static (dateTime, env, recurse) => recurse.Serialize(new DateTimeOffset(dateTime.ToUniversalTime()), env))
            .Override<DedupStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("dedup", recurse, env, step.Scope)
                : CreateInstruction("dedup"))
            .Override<DropStep>(static (_, _, _) => CreateInstruction("drop"))
            .Override<EmitStep>(static (_, _, _) => CreateInstruction("emit"))
            .Override<EStep>(static (step, env, recurse) => CreateInstruction("E", recurse, env, step.Ids))
            .Override<ExplainStep>(static (_, _, _) => CreateInstruction("explain"))
            .Override<FailStep>(static (step, env, recurse) => step.Message is { } message
                ? CreateInstruction("fail", recurse, env, message)
                : CreateInstruction("fail"))
            .Override<FoldStep>(static (_, _, _) => CreateInstruction("fold"))
            .Override<FilterStep.ByLambdaStep>(static (step, env, recurse) => CreateInstruction("filter", recurse, env, step.Lambda))
            .Override<FilterStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("filter", recurse, env, step.Traversal))
            .Override<FlatMapStep>(static (step, env, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
            .Override<GroupStep>(static (_, _, _) => CreateInstruction("group"))
            .Override<GroupStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal))
            .Override<GroupStep.ByKeyStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Override<HasStep>(static (step, env, recurse) => CreateInstruction("has", recurse, env, step.Key))
            .Override<HasKeyStep>(static (step, env, recurse) => CreateInstruction(
                "hasKey",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq" } p
                    ? (object)p.Value
                    : step.Argument))
            .Override<HasPredicateStep>(static (step, env, recurse) => CreateInstruction(
                "has",
                recurse,
                env,
                step.Key,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Override<HasTraversalStep>(static (step, env, recurse) => CreateInstruction("has", recurse, env, step.Key, step.Traversal))
            .Override<HasLabelStep>(static (step, env, recurse) => CreateInstruction("hasLabel", recurse, env, step.Labels))
            .Override<HasNotStep>(static (step, env, recurse) => CreateInstruction("hasNot", recurse, env, step.Key))
            .Override<HasValueStep>(static (step, env, recurse) => CreateInstruction(
                "hasValue",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq" } p
                    ? (object)p.Value
                    : step.Argument))
            .Override<IdentityStep>(static (_, _, _) => CreateInstruction("identity"))
            .Override<IdStep>(static (_, _, _) => CreateInstruction("id"))
            .Override<IGremlinQueryBase>(static (query, env, recurse) =>
            {
                var serialized = recurse.Serialize(
                    query
                        .ToTraversal()
                        .IncludeProjection(env),
                    env);

                return (serialized is Bytecode bytecode)
                    ? new BytecodeGremlinQuery(bytecode)
                    : serialized;
            })
            .Override<InjectStep>(static (step, env, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
            .Override<InEStep>(static (step, env, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
            .Override<InStep>(static (step, env, recurse) => CreateInstruction("in", recurse, env, step.Labels))
            .Override<InVStep>(static (_, _, _) => CreateInstruction("inV"))
            .Override<IsStep>(static (step, env, recurse) => CreateInstruction(
                "is",
                recurse,
                env,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Override<Key>(static (key, env, recurse) => recurse.Serialize(key.RawKey, env))
            .Override<KeyStep>(static (_, _, _) => CreateInstruction("key"))
            .Override<LabelStep>(static (_, _, _) => CreateInstruction("label"))
            .Override<LimitStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                : CreateInstruction("limit", recurse, env, step.Count))
            .Override<LocalStep>(static (step, env, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
            .Override<MaxStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("max", recurse, env, step.Scope)
                : CreateInstruction("max"))
            .Override<MatchStep>(static (step, env, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
            .Override<MapStep>(static (step, env, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
            .Override<MeanStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("mean", recurse, env, step.Scope)
                : CreateInstruction("mean"))
            .Override<Memory<Step>>(static (steps, env, recurse) =>
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
                            Add(recurse.Serialize(step, env));

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

                return recurse.Serialize(byteCode, env);
            })
            .Override<MinStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("min", recurse, env, step.Scope)
                : CreateInstruction("min"))
            .Override<NoneStep>(static (_, _, _) => CreateInstruction("none"))
            .Override<NotStep>(static (step, env, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
            .Override<OptionalStep>(static (step, env, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
            .Override<OptionTraversalStep>(static (step, env, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
            .Override<OrderStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("order", recurse, env, step.Scope)
                : CreateInstruction("order"))
            .Override<OrderStep.ByLambdaStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Lambda))
            .Override<OrderStep.ByMemberStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
            .Override<OrderStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
            .Override<OrStep>(static (step, env, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
            .Override<OutStep>(static (step, env, recurse) => CreateInstruction("out", recurse, env, step.Labels))
            .Override<OutEStep>(static (step, env, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
            .Override<OutVStep>(static (_, _, _) => CreateInstruction("outV"))
            .Override<OtherVStep>(static (_, _, _) => CreateInstruction("otherV"))
            .Override<P>(static (p, env, recurse) =>
            {
                if (p.Value is null)
                    throw new NotSupportedException("Cannot serialize a P-predicate with a null-value.");

                return new P(
                    p.OperatorName,
                    p.Value is IEnumerable enumerable && !env.Model.NativeTypes.Contains(enumerable.GetType())
                        ? enumerable
                            .Cast<object>()
                            .Select(x => recurse.Serialize(x, env))
                            .ToArray()
                        : recurse.Serialize(p.Value, env),
                    p.Other is { } other
                        ? recurse.Serialize(other, env) as P
                        : null);
            })
            .Override<PathStep>(static (_, _, _) => CreateInstruction("path"))
            .Override<ProfileStep>(static (_, _, _) => CreateInstruction("profile"))
            .Override<PropertiesStep>(static (step, env, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
            .Override<PropertyStep.ByKeyStep>(static (step, env, recurse) =>
            {
                static object[] GetPropertyStepArguments(PropertyStep.ByKeyStep propertyStep, IGremlinQuerySerializer recurse, IGremlinQueryEnvironment env)
                {
                    var i = 0;
                    object[] ret;

                    if (propertyStep.Cardinality != null && !T.Id.Equals(propertyStep.Key.RawKey))
                    {
                        ret = new object[propertyStep.MetaProperties.Length * 2 + 3];
                        ret[i++] = recurse.Serialize(propertyStep.Cardinality, env);
                    }
                    else
                        ret = new object[propertyStep.MetaProperties.Length * 2 + 2];

                    ret[i++] = recurse.Serialize(propertyStep.Key, env);
                    ret[i++] = recurse.Serialize(propertyStep.Value, env);

                    for (var j = 0; j < propertyStep.MetaProperties.Length; j++)
                    {
                        ret[i++] = recurse.Serialize(propertyStep.MetaProperties[j].Key, env);
                        ret[i++] = recurse.Serialize(propertyStep.MetaProperties[j].Value, env);
                    }

                    return ret;
                }

                return (T.Id.Equals(step.Key.RawKey) && !Cardinality.Single.Equals(step.Cardinality ?? Cardinality.Single))
                    ? throw new NotSupportedException("Cannot have an id property on non-single cardinality.")
                    : new Instruction("property", GetPropertyStepArguments(step, recurse, env));
            })
            .Override<ProjectStep>(static (step, env, recurse) => CreateInstruction("project", recurse, env, step.Projections))
            .Override<ProjectStep.ByTraversalStep>(static (step, env, recurse) =>
            {
                var traversal = step.Traversal;

                if (traversal.Count == 1 && traversal[0] is LocalStep localStep)
                    traversal = localStep.Traversal;

                return CreateInstruction("by", recurse, env, traversal);
            })
            .Override<ProjectStep.ByKeyStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Override<ProjectVertexStep>(static (_, env, _) => env.Options.GetValue(env.FeatureSet.Supports(VertexFeatures.MetaProperties)
                ? GremlinqOption.VertexProjectionSteps
                : GremlinqOption.VertexProjectionWithoutMetaPropertiesSteps))
            .Override<ProjectEdgeStep>(static (_, env, _) => env.Options.GetValue(GremlinqOption.EdgeProjectionSteps))
            .Override<RangeStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
            .Override<RepeatStep>(static (step, env, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
            .Override<SelectColumnStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.Column))
            .Override<SelectKeysStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.Keys))
            .Override<SelectStepLabelStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
            .Override<SideEffectStep>(static (step, env, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
            .Override<SimplePathStep>(static (_, _, _) => CreateInstruction("simplePath"))
            .Override<SkipStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                : CreateInstruction("skip", recurse, env, step.Count))
            .Override<Step>(static (_, _, _) => Array.Empty<Instruction>())
            .Override<StepLabel>(static (stepLabel, env, recurse) =>
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
                return recurse.Serialize(stepLabelMapping, env);
            })
            .Override<Traversal>(static (traversal, env, recurse) =>
            {
                var steps = ArrayPool<Step>.Shared.Rent(traversal.Count);

                try
                {
                    var stepsMemory = steps
                        .AsMemory()[..traversal.Count];

                    traversal.Steps
                        .AsSpan()
                        .CopyTo(stepsMemory.Span);

                    return recurse.Serialize(stepsMemory, env);
                }
                finally
                {
                    ArrayPool<Step>.Shared.Return(steps);
                }
            })
            .Override<SumStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("sum", recurse, env, step.Scope)
                : CreateInstruction("sum"))
            .Override<TextP>(static (textP, _, _) => textP)
            .Override<TailStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                : CreateInstruction("tail", recurse, env, step.Count))
            .Override<TimesStep>(static (step, env, recurse) => CreateInstruction("times", recurse, env, step.Count))
            .Override<Type>(static (type, _, _) => type)
            .Override<UnfoldStep>(static (_, _, _) => CreateInstruction("unfold"))
            .Override<UnionStep>(static (step, env, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
            .Override<UntilStep>(static (step, env, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
            .Override<ValueStep>(static (_, _, _) => CreateInstruction("value"))
            .Override<ValueMapStep>(static (step, env, recurse) => CreateInstruction("valueMap", recurse, env, step.Keys))
            .Override<ValuesStep>(static (step, env, recurse) => CreateInstruction("values", recurse, env, step.Keys))
            .Override<VStep>(static (step, env, recurse) => CreateInstruction("V", recurse, env, step.Ids))
            .Override<WhereTraversalStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.Traversal))
            .Override<WithStrategiesStep>(static (step, env, recurse) => CreateInstruction("withStrategies", recurse, env, step.Traversal))
            .Override<WithoutStrategiesStep>(static (step, env, recurse) => CreateInstruction("withoutStrategies", recurse, env, step.StrategyTypes))
            .Override<WithSideEffectStep>(static (step, env, recurse) => CreateInstruction("withSideEffect", recurse, env, step.Label, step.Value))
            .Override<WherePredicateStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.Predicate))
            .Override<WherePredicateStep.ByMemberStep>(static (step, env, recurse) => step.Key is { } key
                ? CreateInstruction("by", recurse, env, key)
                : CreateInstruction("by"))
            .Override<WhereStepLabelAndPredicateStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate));

        internal static ISerializedGremlinQuery Serialize(this IGremlinQuerySerializer serializer, IGremlinQueryBase query)
        {
            try
            {
                _stepLabelNames = null;

                var serialized = serializer
                    .Serialize(query, query.AsAdmin().Environment) ?? throw new ArgumentException($"{nameof(query)} did not serialize to a non-null value.");

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

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQuerySerializer recurse, IGremlinQueryEnvironment env, TParam parameter)
        {
            return new(
                name,
                recurse.NullAwareSerialize(parameter, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2>(string name, IGremlinQuerySerializer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2)
        {
            return new(
                name,
                recurse.NullAwareSerialize(parameter1, env),
                recurse.NullAwareSerialize(parameter2, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2, TParam3>(string name, IGremlinQuerySerializer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2, TParam3 parameter3)
        {
            return new(
                name,
                recurse.NullAwareSerialize(parameter1, env),
                recurse.NullAwareSerialize(parameter2, env),
                recurse.NullAwareSerialize(parameter3, env));
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQuerySerializer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters)
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

        private static object? NullAwareSerialize<TParam>(this IGremlinQuerySerializer serializer, TParam maybeParameter, IGremlinQueryEnvironment env)
        {
            return maybeParameter is { } parameter
                ? serializer.Serialize(parameter, env)
                : default;
        }
    }
}
