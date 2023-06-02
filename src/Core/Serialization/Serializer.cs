using System.Collections;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;
using static ExRam.Gremlinq.Core.Serialization.Instructions;
using System.Diagnostics.CodeAnalysis;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Driver;
using Gremlin.Net.Process.Traversal.Strategy;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class Serializer
    {
        private sealed class ByteArrayToStringFallbackConverterFactory : IConverterFactory
        {
            private sealed class ByteArrayToStringFallbackConverter<TTarget> : IConverter<byte[], TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;

                public ByteArrayToStringFallbackConverter(IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                }

                public bool TryConvert(byte[] bytes, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (recurse.TryTransform(Convert.ToBase64String(bytes), _environment, out string? requestedString) && requestedString is TTarget targetString)
                    {
                        value = targetString;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
            {
                if (!environment.SupportsTypeNatively(typeof(byte[])))
                {
                    if (typeof(TSource) == typeof(byte[]) && typeof(TTarget).IsAssignableFrom(typeof(string)))
                        return (IConverter<TSource, TTarget>)(object)new ByteArrayToStringFallbackConverter<TTarget>(environment);
                }

                return default;
            }
        }

        private sealed class TimeSpanToDoubleConverterFactory : IConverterFactory
        {
            private sealed class TimeSpanToDoubleConverter<TTarget> : IConverter<TimeSpan, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;

                public TimeSpanToDoubleConverter(IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                }

                public bool TryConvert(TimeSpan timeSpan, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (recurse.TryTransform(timeSpan.TotalMilliseconds, _environment, out double? requestedDouble) && requestedDouble is TTarget targetDouble)
                    {
                        value = targetDouble;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
            {
                if (!environment.SupportsTypeNatively(typeof(TimeSpan)))
                {
                    if (typeof(TSource) == typeof(TimeSpan) && typeof(TTarget).IsAssignableFrom(typeof(double)))
                        return (IConverter<TSource, TTarget>)(object)new TimeSpanToDoubleConverter<TTarget>(environment);
                }

                return default;
            }
        }

        [ThreadStatic]
        internal static Dictionary<StepLabel, Label>? _stepLabelNames;

        public static readonly ITransformer Default = Transformer.Identity
            .AddBaseConverters()
            .AddDefaultStepConverters();

        public static ITransformer PreferGroovySerialization(this ITransformer serializer) => serializer
            .Add(Chain<IGremlinQueryBase, Bytecode, GroovyGremlinQuery>())
            .Add(Chain<Bytecode, GroovyGremlinQuery, RequestMessage.Builder>())
            .Add(ConverterFactory
                .Create<Bytecode, GroovyGremlinQuery>((query, _, _) => query.ToGroovy()))
            .Add(ConverterFactory
                .Create<GroovyGremlinQuery, RequestMessage.Builder>((query, env, _) => RequestMessage
                    .Build(Tokens.OpsEval)
                    .AddArgument(Tokens.ArgsGremlin, query.Script)
                    .AddArgument(Tokens.ArgsBindings, query.Bindings)
                    .AddAlias(env)));

        private static ITransformer AddBaseConverters(this ITransformer serializer) => serializer
            .Add(Chain<IGremlinQueryBase, RequestMessage.Builder, RequestMessage>())
            .Add(Chain<IGremlinQueryBase, Bytecode, RequestMessage.Builder>())
            .Add(Chain<IGremlinQueryBase, Traversal, Bytecode>())
            .Add(Chain<Bytecode, RequestMessage.Builder, RequestMessage>())

            .Add(ConverterFactory
                .Create<IGremlinQueryBase, Traversal>((query, env, _) =>
                {
                    _stepLabelNames = null;

                    return query
                        .ToTraversal()
                        .IncludeProjection(env);
                })
                .Finally(() => _stepLabelNames = null))
            .Add(ConverterFactory
                .Create<Traversal, Bytecode>((traversal, env, recurse) =>
                {
                    var byteCode = new Bytecode();

                    static void AddTraversal(Traversal traversal, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                    {
                        static void AddStep(Step step, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                        {
                            static void AddInstruction(Instruction instruction, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                            {
                                static void AddInnerInstruction(Instruction instruction, Bytecode byteCode)
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
                                        AddInnerInstruction(expandedInnerInstruction, byteCode);
                                    }
                                }
                                else if (recurse.TryTransform(instruction, env, out Instruction? expandedInstruction))
                                    AddInnerInstruction(expandedInstruction, byteCode);
                                else
                                    AddInnerInstruction(instruction, byteCode);
                            }

                            if (recurse.TryTransform(step, env, out Step[]? expandedSteps))
                            {
                                foreach (var innerExpandedStep in expandedSteps)
                                {
                                    AddStep(innerExpandedStep, byteCode, env, recurse);
                                }
                            }
                            else if (recurse.TryTransform(step, env, out Step? expandedStep) && !ReferenceEquals(step, expandedStep))
                                AddStep(expandedStep, byteCode, env, recurse);
                            else if (recurse.TryTransform(step, env, out Traversal traversal))
                                AddTraversal(traversal, byteCode, env, recurse);
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

                        var span = traversal.Steps;

                        for (var i = 0; i < span.Length; i++)
                        {
                            var j = i + 1;
                            var step = span[i];

                            if (step is AsStep asStep)
                            {
                                AddStep(asStep, byteCode, env, recurse);

                                for (; j < span.Length; j++)
                                {
                                    if (span[j] is SelectStepLabelStep { StepLabels: [var selectedStepLabel] } && ReferenceEquals(asStep.StepLabel, selectedStepLabel))
                                        continue;

                                    if (span[j] is AsStep { StepLabel: { } markedStepLabel } && markedStepLabel.Equals(asStep.StepLabel))
                                        continue;

                                    break;
                                }

                                i = j - 1;
                            }
                            else if (step is IdentityStep identityStep)
                            {
                                AddStep(identityStep, byteCode, env, recurse);

                                for (; j < span.Length; j++)
                                {
                                    if (span[j] is IdentityStep)
                                        continue;

                                    break;
                                }

                                i = j - 1;
                            }
                            else if (step is NoneStep noneStep)
                            {
                                AddStep(noneStep, byteCode, env, recurse);

                                for (; j < span.Length; j++)
                                {
                                    if (span[j] is NoneStep)
                                        continue;

                                    break;
                                }

                                i = j - 1;
                            }
                            else if (step is HasLabelStep hasLabelStep1)
                            {
                                for (; j < span.Length; j++)
                                {
                                    if (span[j] is HasLabelStep hasLabelStep2)
                                    {
                                        hasLabelStep1 = new HasLabelStep(hasLabelStep2.Labels.Intersect(hasLabelStep1.Labels).ToImmutableArray());
                                        continue;
                                    }

                                    break;
                                }

                                AddStep(hasLabelStep1, byteCode, env, recurse);
                                i = j - 1;
                            }
                            else if (step is HasPredicateStep hasPredicateStep1)
                            {
                                for (; j < span.Length; j++)
                                {
                                    if (span[j] is HasPredicateStep hasPredicateStep2 && hasPredicateStep1.Key == hasPredicateStep2.Key)
                                    {
                                        hasPredicateStep1 = new HasPredicateStep(hasPredicateStep2.Key, hasPredicateStep1.Predicate.And(hasPredicateStep2.Predicate));
                                        continue;
                                    }

                                    break;
                                }

                                AddStep(hasPredicateStep1, byteCode, env, recurse);
                                i = j - 1;
                            }
                            else if (step is WithoutStrategiesStep withoutStrategiesStep1)
                            {
                                for (; j < span.Length; j++)
                                {
                                    if (span[j] is WithoutStrategiesStep withoutStrategiesStep2)
                                    {
                                        withoutStrategiesStep1 = new WithoutStrategiesStep(withoutStrategiesStep1.StrategyTypes.Concat(withoutStrategiesStep2.StrategyTypes).Distinct().ToImmutableArray());
                                        continue;
                                    }

                                    break;
                                }

                                AddStep(withoutStrategiesStep1, byteCode, env, recurse);
                                i = j - 1;
                            }
                            else if (step is InjectStep { Elements.Length: 1 } && i == 0 && span.Length > 1 && (span[1] is VStep || span[1] is EStep))
                                continue;
                            else
                                AddStep(step, byteCode, env, recurse);
                        }

                        if (byteCode.StepInstructions.Count == 0)
                            AddStep(IdentityStep.Instance, byteCode, env, recurse);
                    }

                    AddTraversal(traversal, byteCode, env, recurse);

                    return byteCode;
                }))
            .Add(ConverterFactory
                .Create<Bytecode, RequestMessage.Builder>((bytecode, env, _) => RequestMessage
                    .Build(Tokens.OpsBytecode)
                    .Processor(Tokens.ProcessorTraversal)
                    .AddArgument(Tokens.ArgsGremlin, bytecode)
                    .AddAlias(env)))
            .Add(ConverterFactory
                .Create<RequestMessage.Builder, RequestMessage>((builder, _, _) => builder.Create()))
            .Add(ConverterFactory
                .Create<StepLabel, string>((stepLabel, _, _) =>
                {
                    var stepLabelNames = _stepLabelNames ??= new Dictionary<StepLabel, Label>();

                    if (!stepLabelNames.TryGetValue(stepLabel, out var stepLabelMapping))
                    {
                        stepLabelMapping = stepLabel.Identity is string { Length: > 0 } stringIdentity && !stringIdentity.StartsWith('_')
                            ? stringIdentity
                            : stepLabelNames.Count;

                        stepLabelNames.Add(stepLabel, stepLabelMapping);
                    }

                    // ReSharper disable once TailRecursiveCall
                    return stepLabelMapping;
                })
                .AutoRecurse<string>())
            .Add(ConverterFactory
                .Create<DateTime, DateTimeOffset>((dateTime, _, _) => new DateTimeOffset(dateTime.ToUniversalTime()))
                .AutoRecurse<DateTimeOffset>())
            .Add(ConverterFactory
                .Create<Key, T>((key, _, _) => key.RawKey as T)
                .AutoRecurse<T>())
            .Add(ConverterFactory
                .Create<Key, string>((key, _, _) => key.RawKey as string)
                .AutoRecurse<string>())
            .Add(new ByteArrayToStringFallbackConverterFactory())
            .Add(new TimeSpanToDoubleConverterFactory())
            .Add(Create<P, P>((p, env, recurse) =>
            {
                if (p.Value is null)
                    throw new NotSupportedException("Cannot serialize a P-predicate with a null-value.");

                return new P(
                    p.OperatorName,
                    p.Value is IEnumerable enumerable && !env.SupportsType(enumerable.GetType())
                        ? enumerable
                            .Cast<object>()
                            .Select(x => recurse.TransformTo<object>().From(x, env))
                            .ToArray()
                        : recurse.TransformTo<object>().From((object)p.Value, env),
                    p.Other is { } other
                        ? recurse.TransformTo<object>().From(other, env) as P
                        : null);
            }))
            .Add(Create<TextP, TextP>((textP, _, _) => textP))
            .Add(Create<Type, Type>((type, _, _) => type));

        private static ITransformer AddDefaultStepConverters(this ITransformer serializer) => serializer
            .Add<AddEStep>((step, env, recurse) => CreateInstruction("addE", recurse, env, step.Label))
            .Add<AddEStep.ToLabelStep>((step, env, recurse) => CreateInstruction("to", recurse, env, step.StepLabel))
            .Add<AddEStep.ToTraversalStep>((step, env, recurse) => CreateInstruction("to", recurse, env, step.Traversal))
            .Add<AddEStep.FromLabelStep>((step, env, recurse) => CreateInstruction("from", recurse, env, step.StepLabel))
            .Add<AddEStep.FromTraversalStep>((step, env, recurse) => CreateInstruction("from", recurse, env, step.Traversal))
            .Add<AddVStep>((step, env, recurse) => CreateInstruction("addV", recurse, env, step.Label))
            .Add<AndStep>((step, env, recurse) => CreateInstruction("and", recurse, env, step.Traversals))
            .Add<AggregateStep>((step, env, recurse) => step.Scope.Equals(Scope.Global)
                ? CreateInstruction("aggregate", recurse, env, step.StepLabel)
                : CreateInstruction("aggregate", recurse, env, step.Scope, step.StepLabel))
            .Add<AsStep>((step, env, recurse) => CreateInstruction("as", recurse, env, step.StepLabel))
            .Add<BarrierStep>((_, _, _) => barrier)
            .Add<BothStep>((step, env, recurse) => CreateInstruction("both", recurse, env, step.Labels))
            .Add<BothEStep>((step, env, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
            .Add<BothVStep>((_, _, _) => bothV)
            .Add<CapStep>((step, env, recurse) => CreateInstruction("cap", recurse, env, step.StepLabel))
            .Add<ChooseOptionTraversalStep>((step, env, recurse) => CreateInstruction("choose", recurse, env, step.Traversal))
            .Add<ChoosePredicateStep>((step, env, recurse) => step.ElseTraversal is { } elseTraversal
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
            .Add<ChooseTraversalStep>((step, env, recurse) => step.ElseTraversal is { } elseTraversal
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
            .Add<CoalesceStep>((step, env, recurse) => CreateInstruction("coalesce", recurse, env, step.Traversals))
            .Add<CoinStep>((step, env, recurse) => CreateInstruction("coin", recurse, env, step.Probability))
            .Add<ConstantStep>((step, env, recurse) => CreateInstruction("constant", recurse, env, step.Value))
            .Add<CountStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("count", recurse, env, step.Scope)
                : count)
            .Add<CyclicPathStep>((_, _, _) => cyclicPath)
            .Add<DedupStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("dedup", recurse, env, step.Scope)
                : dedup)
            .Add<DropStep>((_, _, _) => drop)
            .Add<EmitStep>((_, _, _) => emit)
            .Add<EStep>((step, env, recurse) => CreateInstruction("E", recurse, env, step.Ids))
            .Add<ExplainStep>((_, _, _) => explain)
            .Add<FailStep>((step, env, recurse) => step.Message is { } message
                ? CreateInstruction("fail", recurse, env, message)
                : fail)
            .Add<FoldStep>((_, _, _) => fold)
            .Add<FilterStep.ByLambdaStep>((step, env, recurse) => CreateInstruction("filter", recurse, env, step.Lambda))
            .Add<FilterStep.ByTraversalStep>((step, env, recurse) => CreateInstruction("filter", recurse, env, step.Traversal))
            .Add<FlatMapStep>((step, env, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
            .Add<GroupStep>((_, _, _) => group)
            .Add<GroupStep.ByTraversalStep>((step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal))
            .Add<GroupStep.ByKeyStep>((step, env, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Add<HasStep>((step, env, recurse) => CreateInstruction("has", recurse, env, step.Key))
            .Add<HasKeyStep>((step, env, recurse) => CreateInstruction(
                "hasKey",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq" } p
                    ? (object)p.Value
                    : step.Argument))
            .Add<HasPredicateStep>((step, env, recurse) => CreateInstruction(
                "has",
                recurse,
                env,
                step.Key,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Add<HasTraversalStep>((step, env, recurse) => CreateInstruction("has", recurse, env, step.Key, step.Traversal))
            .Add<HasLabelStep>((step, env, recurse) => CreateInstruction("hasLabel", recurse, env, step.Labels))
            .Add<HasNotStep>((step, env, recurse) => CreateInstruction("hasNot", recurse, env, step.Key))
            .Add<HasValueStep>((step, env, recurse) => CreateInstruction(
                "hasValue",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq" } p
                    ? (object)p.Value
                    : step.Argument))
            .Add<IdentityStep>((_, _, _) => identity)
            .Add<IdStep>((_, _, _) => id)
            .Add<InjectStep>((step, env, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
            .Add<InEStep>((step, env, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
            .Add<InStep>((step, env, recurse) => CreateInstruction("in", recurse, env, step.Labels))
            .Add<InVStep>((_, _, _) => inV)
            .Add<IsStep>((step, env, recurse) => CreateInstruction(
                "is",
                recurse,
                env,
                step.Predicate.OperatorName == "eq"
                    ? (object)step.Predicate.Value
                    : step.Predicate))
            .Add<KeyStep>((_, _, _) => key)
            .Add<LabelStep>((_, _, _) => label)
            .Add<LimitStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                : CreateInstruction("limit", recurse, env, step.Count))
            .Add<LocalStep>((step, env, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
            .Add<MaxStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("max", recurse, env, step.Scope)
                : max)
            .Add<MatchStep>((step, env, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
            .Add<MapStep>((step, env, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
            .Add<MeanStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("mean", recurse, env, step.Scope)
                : mean)
            .Add<MinStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("min", recurse, env, step.Scope)
                : min)
            .Add<NoneStep>((_, _, _) => none)
            .Add<NotStep>((step, env, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
            .Add<OptionalStep>((step, env, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
            .Add<OptionTraversalStep>((step, env, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
            .Add<OrderStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("order", recurse, env, step.Scope)
                : order)
            .Add<OrderStep.ByLambdaStep>((step, env, recurse) => CreateInstruction("by", recurse, env, step.Lambda))
            .Add<OrderStep.ByMemberStep>((step, env, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
            .Add<OrderStep.ByTraversalStep>((step, env, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
            .Add<OrStep>((step, env, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
            .Add<OutStep>((step, env, recurse) => CreateInstruction("out", recurse, env, step.Labels))
            .Add<OutEStep>((step, env, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
            .Add<OutVStep>((_, _, _) => outV)
            .Add<OtherVStep>((_, _, _) => otherV)
            .Add<PathStep>((_, _, _) => path)
            .Add<ProfileStep>((_, _, _) => profile)
            .Add<PropertiesStep>((step, env, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
            .Add<PropertyStep.ByKeyStep>((step, env, recurse) =>
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
            .Add<ProjectStep>((step, env, recurse) => CreateInstruction("project", recurse, env, step.Projections))
            .Add<ProjectStep.ByTraversalStep>((step, env, recurse) =>
            {
                var traversal = step.Traversal;

                if (traversal is [LocalStep localStep])
                    traversal = localStep.Traversal;

                return CreateInstruction("by", recurse, env, traversal);
            })
            .Add<ProjectStep.ByKeyStep>((step, env, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Add<RangeStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
            .Add<RepeatStep>((step, env, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
            .Add<SelectColumnStep>((step, env, recurse) => CreateInstruction("select", recurse, env, step.Column))
            .Add<SelectKeysStep>((step, env, recurse) => CreateInstruction("select", recurse, env, step.Keys))
            .Add<SelectStepLabelStep>((step, env, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
            .Add<SideEffectStep>((step, env, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
            .Add<SimplePathStep>((_, _, _) => simplePath)
            .Add<SkipStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                : CreateInstruction("skip", recurse, env, step.Count))
            .Add<SumStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("sum", recurse, env, step.Scope)
                : sum)
            .Add<TailStep>((step, env, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                : CreateInstruction("tail", recurse, env, step.Count))
            .Add<TimesStep>((step, env, recurse) => CreateInstruction("times", recurse, env, step.Count))
            .Add<UnfoldStep>((_, _, _) => unfold)
            .Add<UnionStep>((step, env, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
            .Add<UntilStep>((step, env, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
            .Add<ValueStep>((_, _, _) => value)
            .Add<ValueMapStep>((step, env, recurse) => CreateInstruction("valueMap", recurse, env, step.Keys))
            .Add<ValuesStep>((step, env, recurse) => CreateInstruction("values", recurse, env, step.Keys))
            .Add<VStep>((step, env, recurse) => CreateInstruction("V", recurse, env, step.Ids))
            .Add<WhereTraversalStep>((step, env, recurse) => CreateInstruction("where", recurse, env, step.Traversal))
            .Add<WithoutStrategiesStep>((step, env, recurse) => CreateInstruction("withoutStrategies", recurse, env, step.StrategyTypes))
            .Add<WithSideEffectStep>((step, env, recurse) => CreateInstruction("withSideEffect", recurse, env, step.Label, step.Value))
            .Add<WithStrategiesStep>((step, env, recurse) => CreateInstruction(
                "withStrategies",
                recurse,
                env,
                step.Strategies
                    .Select(strategy => recurse
                        .TransformTo<AbstractTraversalStrategy>()
                        .From(strategy, env))
                    .Cast<object>()
                    .ToArray()))
            .Add<WherePredicateStep>((step, env, recurse) => CreateInstruction("where", recurse, env, step.Predicate))
            .Add<WherePredicateStep.ByMemberStep>((step, env, recurse) => step.Key is { } key
                ? CreateInstruction("by", recurse, env, key)
                : by)
            .Add<WhereStepLabelAndPredicateStep>((step, env, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate));

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

        private static Instruction CreateInstruction<TParam>(string name, ITransformer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters) => CreateInstruction(name, recurse, env, parameters.AsSpan());

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

