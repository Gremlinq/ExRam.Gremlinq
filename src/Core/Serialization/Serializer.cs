using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.Serialization.Instructions;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

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

                public bool TryConvert(byte[] bytes, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
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

                public bool TryConvert(TimeSpan timeSpan, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
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

        public static readonly ITransformer Default = Transformer.Empty
            .AddBaseConverters()
            .AddDefaultStepConverters();

        private static ITransformer AddBaseConverters(this ITransformer serializer) => serializer
            .Add(Chain<IGremlinQueryBase, Bytecode, GroovyGremlinScript>())
            .Add(Chain<IGremlinQueryBase, Bytecode, RequestMessage>())
            .Add(Chain<IGremlinQueryBase, Traversal, Bytecode>())

            .Add(ConverterFactory
                .Create<IGremlinQueryBase, Traversal>((query, env, _, _) =>
                {
                    _stepLabelNames = null;

                    return query
                        .ToTraversal()
                        .IncludeProjection(env);
                })
                .Finally(() => _stepLabelNames = null))
            .Add(ConverterFactory
                .Create<Traversal, Bytecode>((traversal, env, _, recurse) =>
                {
                    static void AddTraversal(Traversal traversal, Bytecode byteCode, IGremlinQueryEnvironment env, ITransformer recurse)
                    {
                        AddSteps(traversal.Steps, byteCode, true, env, recurse);
                    }

                    static void AddInstruction(Instruction instruction, Bytecode byteCode, bool isSourceInstruction, IGremlinQueryEnvironment env, ITransformer recurse)
                    {
                        static void AddInnerInstruction(Instruction instruction, Bytecode byteCode, bool isSourceInstruction)
                        {
                            if (isSourceInstruction)
                            {
                                if (byteCode.StepInstructions.Count != 0)
                                    throw new InvalidOperationException("Attempted to add a source instruction after a step instruction has already been added.");

                                byteCode.SourceInstructions.Add(instruction);
                            }
                            else
                                byteCode.StepInstructions.Add(instruction);
                        }

                        if (recurse.TryTransform(instruction, env, out Instruction[]? expandedInnerInstructions))
                        {
                            foreach (var expandedInnerInstruction in expandedInnerInstructions)
                            {
                                AddInnerInstruction(expandedInnerInstruction, byteCode, isSourceInstruction);
                            }
                        }
                        else if (recurse.TryTransform(instruction, env, out Instruction? expandedInstruction))
                            AddInnerInstruction(expandedInstruction, byteCode, isSourceInstruction);
                        else
                            AddInnerInstruction(instruction, byteCode, isSourceInstruction);
                    }

                    static void AddStep(Step step, Bytecode byteCode, bool isSourceStep, IGremlinQueryEnvironment env, ITransformer recurse)
                    {
                        if (recurse.TryTransform(step, env, out Step[]? expandedSteps))
                        {
                            foreach (var innerExpandedStep in expandedSteps)
                            {
                                AddStep(innerExpandedStep, byteCode, isSourceStep, env, recurse);
                            }
                        }
                        else if (recurse.TryTransform(step, env, out Step? expandedStep) && !ReferenceEquals(step, expandedStep))
                            AddStep(expandedStep, byteCode, isSourceStep, env, recurse);
                        else if (recurse.TryTransform(step, env, out Traversal traversal))
                            AddTraversal(traversal, byteCode, env, recurse);
                        else if (recurse.TryTransform(step, env, out Instruction[]? expandedInstructions))
                        {
                            foreach (var expandedInstruction in expandedInstructions)
                            {
                                AddInstruction(expandedInstruction, byteCode, isSourceStep, env, recurse);
                            }
                        }
                        else if (recurse.TryTransform(step, env, out Instruction? expandedInstruction))
                            AddInstruction(expandedInstruction, byteCode, isSourceStep, env, recurse);
                    }

                    static void AddSteps(ReadOnlySpan<Step> steps, Bytecode byteCode, bool isSourceStep, IGremlinQueryEnvironment env, ITransformer recurse)
                    {
                        for (var i = 0; i < steps.Length; i++)
                        {
                            var j = i + 1;
                            var step = steps[i];

                            if (step is ISourceStep)
                            {
                                if (!isSourceStep)
                                    throw new InvalidOperationException("Attempted to add a source instruction after a step instruction has already been added.");
                            }
                            else if (isSourceStep)
                            {
                                AddSteps(steps[i..], byteCode, false, env, recurse);

                                return;
                            }

                            if (step is AsStep asStep)
                            {
                                AddStep(asStep, byteCode, isSourceStep, env, recurse);

                                for (; j < steps.Length; j++)
                                {
                                    if (steps[j] is SelectStepLabelStep { StepLabels: [var selectedStepLabel] } && ReferenceEquals(asStep.StepLabel, selectedStepLabel))
                                        continue;

                                    if (steps[j] is AsStep { StepLabel: { } markedStepLabel } && markedStepLabel.Equals(asStep.StepLabel))
                                        continue;

                                    break;
                                }

                                i = j - 1;
                            }
                            else if (step is IdentityStep identityStep)
                            {
                                AddStep(identityStep, byteCode, isSourceStep, env, recurse);

                                for (; j < steps.Length; j++)
                                {
                                    if (steps[j] is IdentityStep)
                                        continue;

                                    break;
                                }

                                i = j - 1;
                            }
                            else if (step is NoneStep noneStep)
                            {
                                AddStep(noneStep, byteCode, isSourceStep, env, recurse);

                                for (; j < steps.Length; j++)
                                {
                                    if (steps[j] is NoneStep)
                                        continue;

                                    break;
                                }

                                i = j - 1;
                            }
                            else if (step is HasLabelStep hasLabelStep1)
                            {
                                for (; j < steps.Length; j++)
                                {
                                    if (steps[j] is HasLabelStep hasLabelStep2)
                                    {
                                        hasLabelStep1 = new HasLabelStep(hasLabelStep2.Labels.Intersect(hasLabelStep1.Labels).ToImmutableArray());
                                        continue;
                                    }

                                    break;
                                }

                                AddStep(hasLabelStep1, byteCode, isSourceStep, env, recurse);
                                i = j - 1;
                            }
                            else if (step is HasPredicateStep hasPredicateStep1)
                            {
                                for (; j < steps.Length; j++)
                                {
                                    if (steps[j] is HasPredicateStep hasPredicateStep2 && hasPredicateStep1.Key == hasPredicateStep2.Key)
                                    {
                                        hasPredicateStep1 = new HasPredicateStep(hasPredicateStep2.Key, hasPredicateStep1.Predicate.And(hasPredicateStep2.Predicate));
                                        continue;
                                    }

                                    break;
                                }

                                AddStep(hasPredicateStep1, byteCode, isSourceStep, env, recurse);
                                i = j - 1;
                            }
                            else if (!(step is InjectStep { Elements.Length: 1 } && i == 0 && steps.Length > 1 && (steps[1] is VStep || steps[1] is EStep)))
                                AddStep(step, byteCode, isSourceStep, env, recurse);
                        }
                    }

                    return new Bytecode()
                        .Apply(byteCode => AddTraversal(traversal, byteCode, env, recurse));
                }))
            .Add(ConverterFactory
                .Create<Bytecode, GroovyGremlinScript>((query, env, _, _) => query.ToGroovyScript(env)))
            .Add(ConverterFactory
                .Create<GroovyGremlinScript, RequestMessage>((query, env, _, _) => RequestMessage
                    .Build(Tokens.OpsEval)
                    .AddArgument(Tokens.ArgsGremlin, query.Script)
                    .AddArgument(Tokens.ArgsBindings, query.Bindings)
                    .AddAlias(env)
                    .Create()))
            .Add(ConverterFactory
                .Create<Bytecode, RequestMessage>((bytecode, env, _, _) => RequestMessage
                    .Build(Tokens.OpsBytecode)
                    .Processor(Tokens.ProcessorTraversal)
                    .AddArgument(Tokens.ArgsGremlin, bytecode)
                    .AddAlias(env)
                    .Create()))
            .Add(ConverterFactory
                .Create<StepLabel, string>((stepLabel, _, _, _) =>
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
                .Create<DateTime, DateTimeOffset>((dateTime, _, _, _) => new DateTimeOffset(dateTime.ToUniversalTime()))
                .AutoRecurse<DateTimeOffset>())
            .Add(ConverterFactory
                .Create<Key, T>((key, _, _, _) => key.RawKey as T)
                .AutoRecurse<T>())
            .Add(ConverterFactory
                .Create<Key, string>((key, _, _, _) => key.RawKey as string)
                .AutoRecurse<string>())
            .Add(new ByteArrayToStringFallbackConverterFactory())
            .Add(new TimeSpanToDoubleConverterFactory())
            .Add(Create<P, P>((p, env, _, recurse) =>
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
            .Add(Create<TextP, TextP>((textP, _, _, _) => textP))
            .Add(Create<Type, Type>((type, _, _, _) => type));

        private static ITransformer AddDefaultStepConverters(this ITransformer serializer) => serializer
            .Add<AddEStep>((step, env, _, recurse) => CreateInstruction("addE", recurse, env, step.Label))
            .Add<AddEStep.ToLabelStep>((step, env, _, recurse) => CreateInstruction("to", recurse, env, step.StepLabel))
            .Add<AddEStep.ToTraversalStep>((step, env, _, recurse) => CreateInstruction("to", recurse, env, step.Traversal))
            .Add<AddEStep.FromLabelStep>((step, env, _, recurse) => CreateInstruction("from", recurse, env, step.StepLabel))
            .Add<AddEStep.FromTraversalStep>((step, env, _, recurse) => CreateInstruction("from", recurse, env, step.Traversal))
            .Add<AddVStep>((step, env, _, recurse) => CreateInstruction("addV", recurse, env, step.Label))
            .Add<AndStep>((step, env, _, recurse) => CreateInstruction("and", recurse, env, step.Traversals))
            .Add<AggregateStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Global)
                ? CreateInstruction("aggregate", recurse, env, step.StepLabel)
                : CreateInstruction("aggregate", recurse, env, step.Scope, step.StepLabel))
            .Add<AsStep>((step, env, _, recurse) => CreateInstruction("as", recurse, env, step.StepLabel))
            .Add<BarrierStep>((_, _, _, _) => barrier)
            .Add<BothStep>((step, env, _, recurse) => CreateInstruction("both", recurse, env, step.Labels))
            .Add<BothEStep>((step, env, _, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
            .Add<BothVStep>((_, _, _, _) => bothV)
            .Add<CapStep>((step, env, _, recurse) => CreateInstruction("cap", recurse, env, step.StepLabel))
            .Add<ChooseOptionTraversalStep>((step, env, _, recurse) => CreateInstruction("choose", recurse, env, step.Traversal))
            .Add<ChoosePredicateStep>((step, env, _, recurse) => step.ElseTraversal is { } elseTraversal
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
            .Add<ChooseTraversalStep>((step, env, _, recurse) => step.ElseTraversal is { } elseTraversal
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
            .Add<CoalesceStep>((step, env, _, recurse) => CreateInstruction("coalesce", recurse, env, step.Traversals))
            .Add<CoinStep>((step, env, _, recurse) => CreateInstruction("coin", recurse, env, step.Probability))
            .Add<ConstantStep>((step, env, _, recurse) => CreateInstruction("constant", recurse, env, step.Value))
            .Add<CountStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("count", recurse, env, step.Scope)
                : count)
            .Add<CyclicPathStep>((_, _, _, _) => cyclicPath)
            .Add<DedupStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("dedup", recurse, env, step.Scope)
                : dedup)
            .Add<DropStep>((_, _, _, _) => drop)
            .Add<EmitStep>((_, _, _, _) => emit)
            .Add<EStep>((step, env, _, recurse) => CreateInstruction("E", recurse, env, step.Ids))
            .Add<ExplainStep>((_, _, _, _) => explain)
            .Add<FailStep>((step, env, _, recurse) => step.Message is { } message
                ? CreateInstruction("fail", recurse, env, message)
                : fail)
            .Add<FoldStep>((_, _, _, _) => fold)
            .Add<FilterStep.ByTraversalStep>((step, env, _, recurse) => CreateInstruction("filter", recurse, env, step.Traversal))
            .Add<FlatMapStep>((step, env, _, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
            .Add<GroupStep>((_, _, _, _) => group)
            .Add<GroupStep.ByTraversalStep>((step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Traversal))
            .Add<GroupStep.ByKeyStep>((step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Add<HasStep>((step, env, _, recurse) => CreateInstruction("has", recurse, env, step.Key))
            .Add<HasKeyStep>((step, env, _, recurse) => CreateInstruction(
                "hasKey",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq", Value: { } pValue }
                    ? pValue
                    : step.Argument))
            .Add<HasPredicateStep>((step, env, _, recurse) => CreateInstruction(
                "has",
                recurse,
                env,
                step.Key,
                step.Predicate is { OperatorName: "eq", Value: { } predicateValue }
                    ? predicateValue
                    : step.Predicate))
            .Add<HasTraversalStep>((step, env, _, recurse) => CreateInstruction("has", recurse, env, step.Key, step.Traversal))
            .Add<HasLabelStep>((step, env, _, recurse) => CreateInstruction("hasLabel", recurse, env, step.Labels))
            .Add<HasNotStep>((step, env, _, recurse) => CreateInstruction("hasNot", recurse, env, step.Key))
            .Add<HasValueStep>((step, env, _, recurse) => CreateInstruction(
                "hasValue",
                recurse,
                env,
                step.Argument is P { OperatorName: "eq", Value: { } pValue }
                    ? pValue
                    : step.Argument))
            .Add<IdentityStep>((_, _, _, _) => identity)
            .Add<IdStep>((_, _, _, _) => id)
            .Add<InjectStep>((step, env, _, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
            .Add<InEStep>((step, env, _, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
            .Add<InStep>((step, env, _, recurse) => CreateInstruction("in", recurse, env, step.Labels))
            .Add<InVStep>((_, _, _, _) => inV)
            .Add<IsStep>((step, env, _, recurse) => CreateInstruction(
                "is",
                recurse,
                env,
                step.Predicate is { OperatorName: "eq", Value: { } predicateValue }
                    ? predicateValue
                    : step.Predicate))
            .Add<KeyStep>((_, _, _, _) => key)
            .Add<LabelStep>((_, _, _, _) => label)
            .Add<LimitStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                : CreateInstruction("limit", recurse, env, step.Count))
            .Add<LocalStep>((step, env, _, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
            .Add<MaxStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("max", recurse, env, step.Scope)
                : max)
            .Add<MatchStep>((step, env, _, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
            .Add<MapStep>((step, env, _, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
            .Add<MeanStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("mean", recurse, env, step.Scope)
                : mean)
            .Add<MinStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("min", recurse, env, step.Scope)
                : min)
            .Add<NoneStep>((_, _, _, _) => none)
            .Add<NotStep>((step, env, _, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
            .Add<OptionalStep>((step, env, _, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
            .Add<OptionTraversalStep>((step, env, _, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
            .Add<OrderStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("order", recurse, env, step.Scope)
                : order)
            .Add<OrderStep.ByMemberStep>((step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
            .Add<OrderStep.ByTraversalStep>((step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
            .Add<OrStep>((step, env, _, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
            .Add<OutStep>((step, env, _, recurse) => CreateInstruction("out", recurse, env, step.Labels))
            .Add<OutEStep>((step, env, _, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
            .Add<OutVStep>((_, _, _, _) => outV)
            .Add<OtherVStep>((_, _, _, _) => otherV)
            .Add<PathStep>((_, _, _, _) => path)
            .Add<ProfileStep>((_, _, _, _) => profile)
            .Add<PropertiesStep>((step, env, _, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
            .Add<PropertyStep.ByKeyStep>((step, env, _, recurse) =>
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
            .Add<ProjectStep>((step, env, _, recurse) => CreateInstruction("project", recurse, env, step.Projections))
            .Add<ProjectStep.ByTraversalStep>((step, env, _, recurse) =>
            {
                var traversal = step.Traversal;

                if (traversal is [LocalStep localStep])
                    traversal = localStep.Traversal;

                return CreateInstruction("by", recurse, env, traversal);
            })
            .Add<ProjectStep.ByKeyStep>((step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Key))
            .Add<RangeStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
            .Add<RepeatStep>((step, env, _, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
            .Add<SelectColumnStep>((step, env, _, recurse) => CreateInstruction("select", recurse, env, step.Column))
            .Add<SelectKeysStep>((step, env, _, recurse) => CreateInstruction("select", recurse, env, step.Keys))
            .Add<SelectStepLabelStep>((step, env, _, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
            .Add<SideEffectStep>((step, env, _, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
            .Add<SimplePathStep>((_, _, _, _) => simplePath)
            .Add<SkipStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                : CreateInstruction("skip", recurse, env, step.Count))
            .Add<SumStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("sum", recurse, env, step.Scope)
                : sum)
            .Add<TailStep>((step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                : CreateInstruction("tail", recurse, env, step.Count))
            .Add<TimesStep>((step, env, _, recurse) => CreateInstruction("times", recurse, env, step.Count))
            .Add<UnfoldStep>((_, _, _, _) => unfold)
            .Add<UnionStep>((step, env, _, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
            .Add<UntilStep>((step, env, _, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
            .Add<ValueStep>((_, _, _, _) => value)
            .Add<ValueMapStep>((step, env, _, recurse) => CreateInstruction("valueMap", recurse, env, step.Keys))
            .Add<ValuesStep>((step, env, _, recurse) => CreateInstruction("values", recurse, env, step.Keys))
            .Add<VStep>((step, env, _, recurse) => CreateInstruction("V", recurse, env, step.Ids))
            .Add<WhereTraversalStep>((step, env, _, recurse) => CreateInstruction("where", recurse, env, step.Traversal))
            .Add<WithSideEffectStep>((step, env, _, recurse) => CreateInstruction("withSideEffect", recurse, env, step.Label, step.Value))
            .Add<WherePredicateStep>((step, env, _, recurse) => CreateInstruction("where", recurse, env, step.Predicate))
            .Add<WherePredicateStep.ByMemberStep>((step, env, _, recurse) => step.Key is { } key
                ? CreateInstruction("by", recurse, env, key)
                : by)
            .Add<WhereStepLabelAndPredicateStep>((step, env, _, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate))
            .Add<PartitionStrategyStep>((step, env, _, recurse) =>
                CreatePartitionStrategyInstruction(step.PartitionKey, recurse, env));

        private static ITransformer Add<TSource>(this ITransformer serializer, Func<TSource, IGremlinQueryEnvironment, ITransformer, ITransformer, Instruction?> converter) => serializer
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

        private static Instruction CreatePartitionStrategyInstruction(string partitionKey, ITransformer recurse, IGremlinQueryEnvironment env)
        {
            var partitionStrategySyntax = $"PartitionStrategy.build().partitionKey('pk').readPartitions('{partitionKey}').create()";

            return CreateInstruction("withStrategies", recurse, env, partitionStrategySyntax);
        }

        private static object? NullAwareSerialize<TParam>(this ITransformer serializer, TParam maybeParameter, IGremlinQueryEnvironment env) => maybeParameter is { } parameter
            ? serializer.TransformTo<object>().From(parameter, env)
            : default;
    }
}

