using System.Buffers;
using System.Collections;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;
using static ExRam.Gremlinq.Core.Serialization.Instructions;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class Serializer
    {
        [ThreadStatic]
        internal static Dictionary<StepLabel, string>? _stepLabelNames;

        internal static readonly string[] StepLabelNameCache = Enumerable
            .Range(1, 100)
            .Select(static x => "l" + x)
            .ToArray();

        public static readonly ITransformer Default = Transformer.Identity
            .AddBaseConverters()
            .AddDefaultStepConverters();

        public static ITransformer PreferGroovySerialization(this ITransformer serializer) => serializer
            .Add(Create<Bytecode, ISerializedGremlinQuery>((bytecode, env, recurse) => recurse
                .TransformTo<GroovyGremlinQuery>()
                .From(bytecode, env)));

        private static ITransformer AddBaseConverters(this ITransformer serializer) => serializer
            .Add(ConverterFactory
                .Create<IGremlinQueryBase, Traversal>(static (query, env, recurse) => query
                    .ToTraversal()
                    .IncludeProjection(env))
                .AutoRecurse<Traversal>()
                .Finally(() => _stepLabelNames = null))
            .Add(ConverterFactory
                .Create<Traversal, Bytecode>(static (traversal, env, recurse) =>
                {
                    var steps = ArrayPool<Step>.Shared.Rent(traversal.Count);

                    try
                    {
                        var stepsMemory = steps
                            .AsMemory()[..traversal.Count];

                        traversal.Steps
                            .CopyTo(stepsMemory.Span);

                        var j = 0;
                        var span = stepsMemory.Span;

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

                        static void AddStep(Step step, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                        {
                            static void AddInstruction(Instruction instruction, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                            {
                                static void AddInnerInstruction(Instruction instruction, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                                {
                                    if (byteCode.StepInstructions.Count == 0 && instruction.OperatorName.StartsWith("with", StringComparison.OrdinalIgnoreCase))
                                        byteCode.SourceInstructions.Add(instruction);
                                    else
                                        byteCode.StepInstructions.Add(instruction);
                                }

                                if (recurse.TryTransform(instruction, env, out Instruction[]? expandedInnerInstructions))
                                {
                                    foreach (var expandedInnerInstruction in expandedInnerInstructions)
                                    {
                                        AddInnerInstruction(expandedInnerInstruction, byteCode, env, recurse);
                                    }
                                }
                                else if (recurse.TryTransform(instruction, env, out Instruction? expandedInstruction))
                                    AddInnerInstruction(expandedInstruction, byteCode, env, recurse);
                                else
                                    AddInnerInstruction(instruction, byteCode, env, recurse);
                            }

                            if (recurse.TryTransform(step, env, out Step[]? expandedSteps))
                            {
                                foreach (var expandedStep in expandedSteps)
                                {
                                    AddStep(expandedStep, byteCode, env, recurse);
                                }
                            }
                            else if (recurse.TryTransform(step, env, out Instruction[]? expandedInstructions))
                            {
                                foreach (var expandedInstruction in expandedInstructions)
                                {
                                    AddInstruction(expandedInstruction, byteCode, env, recurse);
                                }
                            }
                            else if (recurse.TryTransform(step, env, out Instruction? expandedInstruction))
                                AddInstruction(expandedInstruction, byteCode, env, recurse);
                        }

                        foreach (var step in span)
                        {
                            AddStep(step, byteCode, env, recurse);
                        }

                        if (byteCode.StepInstructions.Count == 0)
                            AddStep(IdentityStep.Instance, byteCode, env, recurse);

                        return byteCode;
                    }
                    finally
                    {
                        ArrayPool<Step>.Shared.Return(steps);
                    }
                })
                .AutoRecurse<Bytecode>())
            .Add(Create<Bytecode, GroovyGremlinQuery>((bytecode, env, recurse) => recurse
                .TransformTo<BytecodeGremlinQuery>()
                .From(bytecode, env)
                .ToGroovy()))
            .Add(ConverterFactory
                .Create<Bytecode, BytecodeGremlinQuery>(static (bytecode, env, recurse) => new BytecodeGremlinQuery(bytecode))
                .AutoRecurse<BytecodeGremlinQuery>())
            .Add(ConverterFactory
                .Create<StepLabel, string>(static (stepLabel, env, recurse) =>
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
                    return stepLabelMapping;
                })
                .AutoRecurse<string>())
            .Add(ConverterFactory
                .Create<DateTime, DateTimeOffset>(static (dateTime, env, recurse) => new DateTimeOffset(dateTime.ToUniversalTime()))
                .AutoRecurse<DateTimeOffset>())
            .Add(ConverterFactory
                .Create<Key, T>(static (key, env, recurse) => key.RawKey as T)
                .AutoRecurse<T>())
            .Add(ConverterFactory
                .Create<Key, string>(static (key, env, recurse) => key.RawKey as string)
                .AutoRecurse<string>())
            .Add(Create<P, P>(static (p, env, recurse) =>
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
            }))
            .Add(Create<TextP, TextP>(static (textP, _, _) => textP))
            .Add(Create<Type, Type>(static (type, _, _) => type));

        private static ITransformer AddDefaultStepConverters(this ITransformer serializer) => serializer
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
            .Add<BarrierStep>(static (_, _, _) => barrier)
            .Add<BothStep>(static (step, env, recurse) => CreateInstruction("both", recurse, env, step.Labels))
            .Add<BothEStep>(static (step, env, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
            .Add<BothVStep>(static (_, _, _) => bothV)
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
                : count)
            .Add<CyclicPathStep>(static (_, _, _) => cyclicPath)
            .Add<DedupStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("dedup", recurse, env, step.Scope)
                : dedup)
            .Add<DropStep>(static (_, _, _) => drop)
            .Add<EmitStep>(static (_, _, _) => emit)
            .Add<EStep>(static (step, env, recurse) => CreateInstruction("E", recurse, env, step.Ids))
            .Add<ExplainStep>(static (_, _, _) => explain)
            .Add<FailStep>(static (step, env, recurse) => step.Message is { } message
                ? CreateInstruction("fail", recurse, env, message)
                : fail)
            .Add<FoldStep>(static (_, _, _) => fold)
            .Add<FilterStep.ByLambdaStep>(static (step, env, recurse) => CreateInstruction("filter", recurse, env, step.Lambda))
            .Add<FilterStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("filter", recurse, env, step.Traversal))
            .Add<FlatMapStep>(static (step, env, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
            .Add<GroupStep>(static (_, _, _) => group)
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
            .Add<IdentityStep>(static (_, _, _) => identity)
            .Add<IdStep>(static (_, _, _) => id)
            .Add<InjectStep>(static (step, env, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
            .Add<InEStep>(static (step, env, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
            .Add<InStep>(static (step, env, recurse) => CreateInstruction("in", recurse, env, step.Labels))
            .Add<InVStep>(static (_, _, _) => inV)
            .Add<IsStep>(static (step, env, recurse) => CreateInstruction(
                "is",
                recurse,
                env,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Add<KeyStep>(static (_, _, _) => key)
            .Add<LabelStep>(static (_, _, _) => label)
            .Add<LimitStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                : CreateInstruction("limit", recurse, env, step.Count))
            .Add<LocalStep>(static (step, env, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
            .Add<MaxStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("max", recurse, env, step.Scope)
                : max)
            .Add<MatchStep>(static (step, env, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
            .Add<MapStep>(static (step, env, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
            .Add<MeanStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("mean", recurse, env, step.Scope)
                : mean)
            .Add<MinStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("min", recurse, env, step.Scope)
                : min)
            .Add<NoneStep>(static (_, _, _) => none)
            .Add<NotStep>(static (step, env, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
            .Add<OptionalStep>(static (step, env, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
            .Add<OptionTraversalStep>(static (step, env, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
            .Add<OrderStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("order", recurse, env, step.Scope)
                : order)
            .Add<OrderStep.ByLambdaStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Lambda))
            .Add<OrderStep.ByMemberStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
            .Add<OrderStep.ByTraversalStep>(static (step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
            .Add<OrStep>(static (step, env, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
            .Add<OutStep>(static (step, env, recurse) => CreateInstruction("out", recurse, env, step.Labels))
            .Add<OutEStep>(static (step, env, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
            .Add<OutVStep>(static (_, _, _) => outV)
            .Add<OtherVStep>(static (_, _, _) => otherV)
            .Add<PathStep>(static (_, _, _) => path)
            .Add<ProfileStep>(static (_, _, _) => profile)
            .Add<PropertiesStep>(static (step, env, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
            .Add<PropertyStep.ByKeyStep>(static (step, env, recurse) =>
            {
                if (T.Id.Equals(step.Key.RawKey) && !Cardinality.Single.Equals(step.Cardinality ?? Cardinality.Single))
                    throw new NotSupportedException("Cannot have an id property on non-single cardinality.");

                var i = 0;
                object[] parameters;

                if (step.Cardinality != null && !T.Id.Equals(step.Key.RawKey))
                {
                    parameters = new object[step.MetaProperties.Length * 2 + 3];
                    parameters[i++] = step.Cardinality;
                }
                else
                    parameters = new object[step.MetaProperties.Length * 2 + 2];

                parameters[i++] = step.Key;
                parameters[i++] = step.Value;

                for (var j = 0; j < step.MetaProperties.Length; j++)
                {
                    parameters[i++] = step.MetaProperties[j].Key;
                    parameters[i++] = step.MetaProperties[j].Value;
                }

                return CreateInstruction("property", recurse, env, parameters);
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
            .Add(Create<ProjectVertexStep, Traversal>(static (_, env, _) => env.Options.GetValue(env.FeatureSet.Supports(VertexFeatures.MetaProperties)
                ? GremlinqOption.VertexProjectionSteps
                : GremlinqOption.VertexProjectionWithoutMetaPropertiesSteps)))
            .Add(Create<ProjectEdgeStep, Traversal>(static (_, env, _) => env.Options.GetValue(GremlinqOption.EdgeProjectionSteps)))
            .Add<RangeStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
            .Add<RepeatStep>(static (step, env, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
            .Add<SelectColumnStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.Column))
            .Add<SelectKeysStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.Keys))
            .Add<SelectStepLabelStep>(static (step, env, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
            .Add<SideEffectStep>(static (step, env, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
            .Add<SimplePathStep>(static (_, _, _) => simplePath)
            .Add<SkipStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                : CreateInstruction("skip", recurse, env, step.Count))
            .Add<SumStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("sum", recurse, env, step.Scope)
                : sum)
            .Add<TailStep>(static (step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                : CreateInstruction("tail", recurse, env, step.Count))
            .Add<TimesStep>(static (step, env, recurse) => CreateInstruction("times", recurse, env, step.Count))
            .Add<UnfoldStep>(static (_, _, _) => unfold)
            .Add<UnionStep>(static (step, env, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
            .Add<UntilStep>(static (step, env, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
            .Add<ValueStep>(static (_, _, _) => value)
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
                : by)
            .Add<WhereStepLabelAndPredicateStep>(static (step, env, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate));

        private static ITransformer Add<TSource>(this ITransformer serializer, Func<TSource, IGremlinQueryEnvironment, ITransformer, Instruction?> converter) => serializer
            .Add(Create(converter));

        private static Instruction CreateInstruction<TParam>(string name, ITransformer recurse, IGremlinQueryEnvironment env, TParam parameter) => new(
            name,
            recurse.NullAwareSerialize(parameter, env));

        private static Instruction CreateInstruction<TParam1, TParam2>(string name, ITransformer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2) => new(
            name,
            recurse.NullAwareSerialize(parameter1, env),
            recurse.NullAwareSerialize(parameter2, env));

        private static Instruction CreateInstruction<TParam1, TParam2, TParam3>(string name, ITransformer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2, TParam3 parameter3) => new(
            name,
            recurse.NullAwareSerialize(parameter1, env),
            recurse.NullAwareSerialize(parameter2, env),
            recurse.NullAwareSerialize(parameter3, env));

        private static Instruction CreateInstruction(string name, ITransformer recurse, IGremlinQueryEnvironment env, object[] parameters) => CreateInstruction<object>(name, recurse, env, parameters.AsSpan());

        private static Instruction CreateInstruction<TParam>(string name, ITransformer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters) => CreateInstruction<TParam>(name, recurse, env, parameters.AsSpan());

        private static Instruction CreateInstruction<TParam>(string name, ITransformer recurse, IGremlinQueryEnvironment env, ReadOnlySpan<TParam> parameters)
        {
            if (parameters.Length == 0)
                return new Instruction(name);

            var arguments = new object?[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                arguments[i] = recurse.NullAwareSerialize(parameters[i], env);
            }

            return new Instruction(name, arguments);
        }

        private static object? NullAwareSerialize<TParam>(this ITransformer serializer, TParam maybeParameter, IGremlinQueryEnvironment env) => maybeParameter is { } parameter
            ? serializer.TransformTo<object>().From(parameter, env)
            : default;
    }
}

