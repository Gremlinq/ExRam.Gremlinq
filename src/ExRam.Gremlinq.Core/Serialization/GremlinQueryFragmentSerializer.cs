using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class GremlinQueryFragmentSerializer
    {
        private sealed class GremlinQueryFragmentSerializerImpl : IGremlinQueryFragmentSerializer
        {
            private static readonly MethodInfo CreateFuncMethod1 = typeof(GremlinQueryFragmentSerializerImpl).GetMethod(nameof(CreateFunc1), BindingFlags.NonPublic | BindingFlags.Static)!;
            private static readonly MethodInfo CreateFuncMethod2 = typeof(GremlinQueryFragmentSerializerImpl).GetMethod(nameof(CreateFunc2), BindingFlags.NonPublic | BindingFlags.Static)!;

            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new();

            public GremlinQueryFragmentSerializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment)
            {
                return TryGetSerializer(typeof(TFragment), fragment!.GetType()) is BaseGremlinQueryFragmentSerializerDelegate<TFragment> del
                    ? del(fragment, gremlinQueryEnvironment, this)
                    : fragment;
            }

            public IGremlinQueryFragmentSerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer)
            {
                return new GremlinQueryFragmentSerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        TryGetSerializer(typeof(TFragment), typeof(TFragment)) is BaseGremlinQueryFragmentSerializerDelegate<TFragment> existingFragmentSerializer
                            ? (fragment, env, _, recurse) => serializer(fragment, env, existingFragmentSerializer, recurse)
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
                                    ? CreateFuncMethod1
                                        .MakeGenericMethod(staticType)
                                    : CreateFuncMethod2
                                        .MakeGenericMethod(staticType, effectiveType);

                                return (Delegate)method
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

            private static BaseGremlinQueryFragmentSerializerDelegate<TStatic> CreateFunc1<TStatic>(GremlinQueryFragmentSerializerDelegate<TStatic> del) => (fragment, environment, recurse) => del(fragment!, environment, static (_, e, s) => _!, recurse);

            private static BaseGremlinQueryFragmentSerializerDelegate<TStatic> CreateFunc2<TStatic, TEffective>(GremlinQueryFragmentSerializerDelegate<TEffective> del) => (fragment, environment, recurse) => del((TEffective)(object)fragment!, environment, static (_, e, s) => _!, recurse);
        }

        public static readonly IGremlinQueryFragmentSerializer Identity = new GremlinQueryFragmentSerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
        public static readonly IGremlinQueryFragmentSerializer Default = Identity.UseDefaultGremlinStepSerializationHandlers();

        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new();

        private static readonly HashSet<string> SourceStepNames = new()
        {
            "withStrategies",
            "withoutStrategies",
            "withSideEffect"
        };

        public static IGremlinQueryFragmentSerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQueryFragmentSerializer fragmentSerializer)
        {
            return fragmentSerializer
                .Override<AddEStep>(static (step, env, _, recurse) => CreateInstruction("addE", recurse, env, step.Label))
                .Override<AddEStep.ToLabelStep>(static (step, env, _, recurse) => CreateInstruction("to", recurse, env, step.StepLabel))
                .Override<AddEStep.ToTraversalStep>(static (step, env, _, recurse) => CreateInstruction("to", recurse, env, step.Traversal))
                .Override<AddEStep.FromLabelStep>(static (step, env, _, recurse) => CreateInstruction("from", recurse, env, step.StepLabel))
                .Override<AddEStep.FromTraversalStep>(static (step, env, _, recurse) => CreateInstruction("from", recurse, env, step.Traversal))
                .Override<AddVStep>(static (step, env, _, recurse) => CreateInstruction("addV", recurse, env, step.Label))
                .Override<AndStep>(static (step, env, _, recurse) => CreateInstruction("and", recurse, env, step.Traversals))
                .Override<AggregateStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Global)
                    ? CreateInstruction("aggregate", recurse, env, step.StepLabel)
                    : CreateInstruction("aggregate", recurse, env, step.Scope, step.StepLabel))
                .Override<AsStep>(static (step, env, _, recurse) => CreateInstruction("as", recurse, env, step.StepLabel))
                .Override<BarrierStep>(static (_, _, _, _) => CreateInstruction("barrier"))
                .Override<BothStep>(static (step, env, _, recurse) => CreateInstruction("both", recurse, env, step.Labels))
                .Override<BothEStep>(static (step, env, _, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
                .Override<BothVStep>(static (_, _, _, _) => CreateInstruction("bothV"))
                .Override<CapStep>(static (step, env, _, recurse) => CreateInstruction("cap", recurse, env, step.StepLabel))
                .Override<ChooseOptionTraversalStep>(static (step, env, _, recurse) => CreateInstruction("choose", recurse, env, step.Traversal))
                .Override<ChoosePredicateStep>(static (step, env, _, recurse) => step.ElseTraversal is { } elseTraversal
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
                .Override<ChooseTraversalStep>(static (step, env, _, recurse) => step.ElseTraversal is { } elseTraversal
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
                .Override<CoalesceStep>(static (step, env, _, recurse) => CreateInstruction("coalesce", recurse, env, step.Traversals))
                .Override<CoinStep>(static (step, env, _, recurse) => CreateInstruction("coin", recurse, env, step.Probability))
                .Override<ConstantStep>(static (step, env, _, recurse) => CreateInstruction("constant", recurse, env, step.Value))
                .Override<CountStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("count", recurse, env, step.Scope)
                    : CreateInstruction("count"))
                .Override<CyclicPathStep>(static (_, _, _, _) => CreateInstruction("cyclicPath"))
                .Override<DateTime>(static (dateTime, env, _, recurse) => recurse.Serialize(new DateTimeOffset(dateTime.ToUniversalTime()), env))
                .Override<DedupStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("dedup", recurse, env, step.Scope)
                    : CreateInstruction("dedup"))
                .Override<DropStep>(static (_, _, _, _) => CreateInstruction("drop"))
                .Override<EmitStep>(static (_, _, _, _) => CreateInstruction("emit"))
                .Override<EStep>(static (step, env, _, recurse) => CreateInstruction("E", recurse, env, step.Ids))
                .Override<ExplainStep>(static (_, _, _, _) => CreateInstruction("explain"))
                .Override<FailStep>(static (step, env, _, recurse) => step.Message is { } message
                    ? CreateInstruction("fail", recurse, env, message)
                    : CreateInstruction("fail"))
                .Override<FoldStep>(static (_, _, _, _) => CreateInstruction("fold"))
                .Override<FilterStep.ByLambdaStep>(static (step, env, _, recurse) => CreateInstruction("filter", recurse, env, step.Lambda))
                .Override<FilterStep.ByTraversalStep>(static (step, env, _, recurse) => CreateInstruction("filter", recurse, env, step.Traversal))
                .Override<FlatMapStep>(static (step, env, _, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
                .Override<GroupStep>(static (_, _, _, _) => CreateInstruction("group"))
                .Override<GroupStep.ByTraversalStep>(static (step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Traversal))
                .Override<GroupStep.ByKeyStep>(static (step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Key))
                .Override<HasKeyStep>(static (step, env, _, recurse) => CreateInstruction(
                    "hasKey",
                    recurse,
                    env,
                    step.Argument is P { OperatorName: "eq" } p
                        ? (object)p.Value
                        : step.Argument))
                .Override<HasPredicateStep>(static (step, env, _, recurse) =>
                {
                    static Step UnwindHasPredicateStep(HasPredicateStep step)
                    {
                        if (step.Predicate is { } p && p.ContainsNullArgument())
                        {
                            if (p.IsAnd() || p.IsOr())
                            {
                                var replacement = new Traversal[]
                                {
                                    UnwindHasPredicateStep(new HasPredicateStep(step.Key, p.Value is P innerP ? innerP : P.Eq(p.Value))),
                                    UnwindHasPredicateStep(new HasPredicateStep(step.Key, p.Other))
                                };

                                if (p.IsOr())
                                    return new OrStep(replacement);

                                if (p.IsAnd())
                                    return new AndStep(replacement);
                            }
                        }

                        return step;
                    }

                    if (UnwindHasPredicateStep(step) is { } unwound && unwound != step)
                        return recurse.Serialize(unwound, env);

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
                        ? CreateInstruction(stepName, recurse, env, step.Key, argument)
                        : CreateInstruction(stepName, recurse, env, step.Key);
                })
                .Override<HasTraversalStep>(static (step, env, _, recurse) => CreateInstruction("has", recurse, env, step.Key, step.Traversal))
                .Override<HasLabelStep>(static (step, env, _, recurse) => CreateInstruction("hasLabel", recurse, env, step.Labels))
                .Override<HasNotStep>(static (step, env, _, recurse) => CreateInstruction("hasNot", recurse, env, step.Key))
                .Override<HasValueStep>(static (step, env, _, recurse) => CreateInstruction(
                    "hasValue",
                    recurse,
                    env,
                    step.Argument is P { OperatorName: "eq" } p
                        ? (object)p.Value
                        : step.Argument))
                .Override<IdentityStep>(static (_, _, _, _) => CreateInstruction("identity"))
                .Override<IdStep>(static (_, _, _, _) => CreateInstruction("id"))
                .Override<IGremlinQueryBase>(static (query, env, _, recurse) =>
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
                .Override<InjectStep>(static (step, env, _, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
                .Override<InEStep>(static (step, env, _, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
                .Override<InStep>(static (step, env, _, recurse) => CreateInstruction("in", recurse, env, step.Labels))
                .Override<InVStep>(static (_, _, _, _) => CreateInstruction("inV"))
                .Override<IsStep>(static (step, env, _, recurse) => CreateInstruction(
                    "is",
                    recurse,
                    env,
                    step.Predicate.OperatorName == "eq"
                        ? (object)step.Predicate.Value
                        : step.Predicate))
                .Override<Key>(static (key, env, _, recurse) => recurse.Serialize(key.RawKey, env))
                .Override<KeyStep>(static (_, _, _, _) => CreateInstruction("key"))
                .Override<LabelStep>(static (_, _, _, _) => CreateInstruction("label"))
                .Override<LimitStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                    : CreateInstruction("limit", recurse, env, step.Count))
                .Override<LocalStep>(static (step, env, _, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
                .Override<MaxStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("max", recurse, env, step.Scope)
                    : CreateInstruction("max"))
                .Override<MatchStep>(static (step, env, _, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
                .Override<MapStep>(static (step, env, _, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
                .Override<MeanStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("mean", recurse, env, step.Scope)
                    : CreateInstruction("mean"))
                .Override<MinStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("min", recurse, env, step.Scope)
                    : CreateInstruction("min"))
                .Override<NoneStep>(static (_, _, _, _) => CreateInstruction("none"))
                .Override<NotStep>(static (step, env, _, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
                .Override<OptionalStep>(static (step, env, _, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
                .Override<OptionTraversalStep>(static (step, env, _, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
                .Override<OrderStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("order", recurse, env, step.Scope)
                    : CreateInstruction("order"))
                .Override<OrderStep.ByLambdaStep>(static (step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Lambda))
                .Override<OrderStep.ByMemberStep>(static (step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
                .Override<OrderStep.ByTraversalStep>(static (step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
                .Override<OrStep>(static (step, env, _, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
                .Override<OutStep>(static (step, env, _, recurse) => CreateInstruction("out", recurse, env, step.Labels))
                .Override<OutEStep>(static (step, env, _, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
                .Override<OutVStep>(static (_, _, _, _) => CreateInstruction("outV"))
                .Override<OtherVStep>(static (_, _, _, _) => CreateInstruction("otherV"))
                .Override<P>(static (p, env, _, recurse) =>
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
                .Override<PathStep>(static (_, _, _, _) => CreateInstruction("path"))
                .Override<ProfileStep>(static (_, _, _, _) => CreateInstruction("profile"))
                .Override<PropertiesStep>(static (step, env, _, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
                .Override<PropertyStep.ByKeyStep>(static (step, env, _, recurse) =>
                {
                    static IEnumerable<object?> GetPropertyStepArguments(PropertyStep.ByKeyStep propertyStep, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env)
                    {
                        if (propertyStep.Cardinality != null && !T.Id.Equals(propertyStep.Key.RawKey))
                            yield return propertyStep.Cardinality;

                        yield return propertyStep.Key;
                        yield return recurse.Serialize(propertyStep.Value, env);

                        foreach (var metaProperty in propertyStep.MetaProperties)
                        {
                            yield return metaProperty.Key;
                            yield return recurse.Serialize(metaProperty.Value, env);
                        }
                    }

                    return (T.Id.Equals(step.Key.RawKey) && !Cardinality.Single.Equals(step.Cardinality ?? Cardinality.Single))
                        ? throw new NotSupportedException("Cannot have an id property on non-single cardinality.")
                        : CreateInstruction("property", recurse, env, GetPropertyStepArguments(step, recurse, env));
                })
                .Override<ProjectStep>(static (step, env, _, recurse) => CreateInstruction("project", recurse, env, step.Projections))
                .Override<ProjectStep.ByTraversalStep>(static (step, env, _, recurse) =>
                {
                    var traversal = step.Traversal;

                    if (traversal.Count == 1 && traversal[0] is LocalStep localStep)
                        traversal = localStep.Traversal;

                    return CreateInstruction("by", recurse, env, traversal);
                })
                .Override<ProjectStep.ByKeyStep>(static (step, env, _, recurse) => CreateInstruction("by", recurse, env, step.Key))
                .Override<ProjectVertexStep>(static (_, env, _, _) => env.Options.GetValue(env.FeatureSet.Supports(VertexFeatures.MetaProperties)
                    ? GremlinqOption.VertexProjectionSteps
                    : GremlinqOption.VertexProjectionWithoutMetaPropertiesSteps))
                .Override<ProjectEdgeStep>(static (_, env, _, _) => env.Options.GetValue(GremlinqOption.EdgeProjectionSteps))
                .Override<RangeStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                    : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
                .Override<RepeatStep>(static (step, env, _, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
                .Override<SelectColumnStep>(static (step, env, _, recurse) => CreateInstruction("select", recurse, env, step.Column))
                .Override<SelectKeysStep>(static (step, env, _, recurse) => CreateInstruction("select", recurse, env, step.Keys))
                .Override<SelectStepLabelStep>(static (step, env, _, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
                .Override<SideEffectStep>(static (step, env, _, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
                .Override<SimplePathStep>(static (_, _, _, _) => CreateInstruction("simplePath"))
                .Override<SkipStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                    : CreateInstruction("skip", recurse, env, step.Count))
                .Override<Step>(static (_, _, _, _) => Array.Empty<Instruction>())
                .Override<Traversal>(static (traversal, env, _, recurse) =>
                {
                    var byteCode = new Bytecode();
                    IReadOnlyList<Step> steps = traversal;

                    void Add(object? obj)
                    {
                        switch (obj)
                        {
                            case Instruction instruction:
                            {
                                if (SourceStepNames.Contains(instruction.OperatorName))
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

                    Add(steps);

                    if (byteCode.StepInstructions.Count == 0)
                        Add(IdentityStep.Instance);

                    return recurse.Serialize(byteCode, env);
                })
                .Override<SumStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("sum", recurse, env, step.Scope)
                    : CreateInstruction("sum"))
                .Override<TextP>(static (textP, _, _, _) => textP)
                .Override<TailStep>(static (step, env, _, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                    : CreateInstruction("tail", recurse, env, step.Count))
                .Override<TimesStep>(static (step, env, _, recurse) => CreateInstruction("times", recurse, env, step.Count))
                .Override<Type>(static (type, _, _, _) => type)
                .Override<UnfoldStep>(static (_, _, _, _) => CreateInstruction("unfold"))
                .Override<UnionStep>(static (step, env, _, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
                .Override<UntilStep>(static (step, env, _, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
                .Override<ValueStep>(static (_, _, _, _) => CreateInstruction("value"))
                .Override<ValueMapStep>(static (step, env, _, recurse) => CreateInstruction("valueMap", recurse, env, step.Keys))
                .Override<ValuesStep>(static (step, env, _, recurse) => CreateInstruction("values", recurse, env, step.Keys))
                .Override<VStep>(static (step, env, _, recurse) => CreateInstruction("V", recurse, env, step.Ids))
                .Override<WhereTraversalStep>(static (step, env, _, recurse) => CreateInstruction("where", recurse, env, step.Traversal))
                .Override<WithStrategiesStep>(static (step, env, _, recurse) => CreateInstruction("withStrategies", recurse, env, step.Traversal))
                .Override<WithoutStrategiesStep>(static (step, env, _, recurse) => CreateInstruction("withoutStrategies", recurse, env, step.StrategyTypes))
                .Override<WithSideEffectStep>(static (step, env, _, recurse) => CreateInstruction("withSideEffect", recurse, env, step.Label, step.Value))
                .Override<WherePredicateStep>(static (step, env, _, recurse) => CreateInstruction("where", recurse, env, step.Predicate))
                .Override<WherePredicateStep.ByMemberStep>(static (step, env, _, recurse) => step.Key != null
                    ? CreateInstruction("by", recurse, env, step.Key)
                    : CreateInstruction("by"))
                .Override<WhereStepLabelAndPredicateStep>(static (step, env, _, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate));
        }

        private static Instruction CreateInstruction(string name)
        {
            return SimpleInstructions.GetOrAdd(
                name,
                static closure => new Instruction(closure));
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam parameter)
        {
            return new(
                name,
                recurse.Serialize(parameter, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2)
        {
            return new(
                name,
                recurse.Serialize(parameter1, env),
                recurse.Serialize(parameter2, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2, TParam3>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2, TParam3 parameter3)
        {
            return new(
                name,
                recurse.Serialize(parameter1, env),
                recurse.Serialize(parameter2, env),
                recurse.Serialize(parameter3, env));
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam[] parameters)
        {
            if (parameters.Length == 0)
                return CreateInstruction(name);

            var data = parameters
                .Select(x => recurse.Serialize(x, env))
                .Where(static x => x != null)
                .Select(static x => x!)
                .ToArray();

            return new Instruction(name, data);
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters)
        {
            if (parameters.Length == 0)
                return CreateInstruction(name);

            var data = parameters
                .Select(x => recurse.Serialize(x, env))
                .Where(static x => x != null)
                .Select(static x => x!)
                .ToArray();

            return new Instruction(name, data);
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, IEnumerable<TParam> parameters)
        {
            return new(
                name,
                parameters
                    .Select(x => recurse.Serialize(x, env))
                    .ToArray());
        }
    }
}
