using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
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

            public object? Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment)
            {
                return TryGetSerializer(typeof(TFragment), fragment!.GetType()) is Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object?> del
                    ? del(fragment, gremlinQueryEnvironment, this)
                    : fragment;
            }

            public IGremlinQueryFragmentSerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer)
            {
                return new GremlinQueryFragmentSerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        TryGetSerializer(typeof(TFragment), typeof(TFragment)) is Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object?> existingFragmentSerializer
                            ? (fragment, env, _, recurse) => serializer(fragment, env, existingFragmentSerializer, recurse)
                            : serializer));
            }

            private Delegate? TryGetSerializer(Type staticType, Type actualType)
            {
                return _fastDict
                    .GetOrAdd(
                        (staticType, actualType),
                        (typeTuple, @this) =>
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

            private static Func<TStatic, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object?> CreateFunc1<TStatic>(GremlinQueryFragmentSerializerDelegate<TStatic> del) => (fragment, environment, recurse) => del(fragment!, environment, (_, e, s) => _, recurse);

            private static Func<TStatic, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object?> CreateFunc2<TStatic, TEffective>(GremlinQueryFragmentSerializerDelegate<TEffective> del) => (fragment, environment, recurse) => del((TEffective)(object)fragment!, environment, (_, e, s) => _, recurse);
        }

        public static readonly IGremlinQueryFragmentSerializer Identity = new GremlinQueryFragmentSerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);
        public static readonly IGremlinQueryFragmentSerializer Default = Identity.UseDefaultGremlinStepSerializationHandlers();

        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new();
        private static readonly ImmutableArray<Step> IdentitySteps = ImmutableArray.Create((Step)IdentityStep.Instance);

        private static readonly HashSet<string> SourceStepNames = new()
        {
            "withStrategies",
            "withoutStrategies",
            "withSideEffect"
        };

        public static IGremlinQueryFragmentSerializer UseDefaultGremlinStepSerializationHandlers(this IGremlinQueryFragmentSerializer fragmentSerializer)
        {
            return fragmentSerializer
                .Override<AddEStep>((step, env, overridden, recurse) => CreateInstruction("addE", recurse, env, step.Label))
                .Override<AddEStep.ToLabelStep>((step, env, overridden, recurse) => CreateInstruction("to", recurse, env, step.StepLabel))
                .Override<AddEStep.ToTraversalStep>((step, env, overridden, recurse) => CreateInstruction("to", recurse, env, step.Traversal))
                .Override<AddEStep.FromLabelStep>((step, env, overridden, recurse) => CreateInstruction("from", recurse, env, step.StepLabel))
                .Override<AddEStep.FromTraversalStep>((step, env, overridden, recurse) => CreateInstruction("from", recurse, env, step.Traversal))
                .Override<AddVStep>((step, env, overridden, recurse) => CreateInstruction("addV", recurse, env, step.Label))
                .Override<AndStep>((step, env, overridden, recurse) => CreateInstruction("and", recurse, env, step.Traversals))
                .Override<AggregateStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("aggregate", recurse, env, step.StepLabel)
                    : CreateInstruction("aggregate", recurse, env, step.Scope, step.StepLabel))
                .Override<AsStep>((step, env, overridden, recurse) => CreateInstruction("as", recurse, env, step.StepLabel))
                .Override<BarrierStep>((step, env, overridden, recurse) => CreateInstruction("barrier"))
                .Override<BothStep>((step, env, overridden, recurse) => CreateInstruction("both", recurse, env, step.Labels))
                .Override<BothEStep>((step, env, overridden, recurse) => CreateInstruction("bothE", recurse, env, step.Labels))
                .Override<BothVStep>((step, env, overridden, recurse) => CreateInstruction("bothV"))
                .Override<CapStep>((step, env, overridden, recurse) => CreateInstruction("cap", recurse, env, step.StepLabel))
                .Override<ChooseOptionTraversalStep>((step, env, overridden, recurse) => CreateInstruction("choose", recurse, env, step.Traversal))
                .Override<ChoosePredicateStep>((step, env, overridden, recurse) => step.ElseTraversal is { } elseTraversal
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
                .Override<ChooseTraversalStep>((step, env, overridden, recurse) => step.ElseTraversal is { } elseTraversal
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
                .Override<CoalesceStep>((step, env, overridden, recurse) => CreateInstruction("coalesce", recurse, env, step.Traversals))
                .Override<CoinStep>((step, env, overridden, recurse) => CreateInstruction("coin", recurse, env, step.Probability))
                .Override<ConstantStep>((step, env, overridden, recurse) => CreateInstruction("constant", recurse, env, step.Value))
                .Override<CountStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("count", recurse, env, step.Scope)
                    : CreateInstruction("count"))
                .Override<CyclicPathStep>((step, env, overridden, recurse) => CreateInstruction("cyclicPath"))
                .Override<DateTime>((dateTime, env, overridden, recurse) => recurse.Serialize(new DateTimeOffset(dateTime.ToUniversalTime()), env))
                .Override<DedupStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("dedup", recurse, env, step.Scope)
                    : CreateInstruction("dedup"))
                .Override<DropStep>((step, env, overridden, recurse) => CreateInstruction("drop"))
                .Override<EmitStep>((step, env, overridden, recurse) => CreateInstruction("emit"))
                .Override<EStep>((step, env, overridden, recurse) => CreateInstruction("E", recurse, env, step.Ids))
                .Override<ExplainStep>((step, env, overridden, recurse) => CreateInstruction("explain"))
                .Override<FoldStep>((step, env, overridden, recurse) => CreateInstruction("fold"))
                .Override<FilterStep>((step, env, overridden, recurse) => CreateInstruction("filter", recurse, env, step.Lambda))
                .Override<FlatMapStep>((step, env, overridden, recurse) => CreateInstruction("flatMap", recurse, env, step.Traversal))
                .Override<GroupStep>((step, env, overridden, recurse) => CreateInstruction("group"))
                .Override<GroupStep.ByTraversalStep>((step, env, overridden, recurse) => CreateInstruction("by", recurse, env, step.Traversal))
                .Override<GroupStep.ByKeyStep>((step, env, overridden, recurse) => CreateInstruction("by", recurse, env, step.Key))
                .Override<HasKeyStep>((step, env, overridden, recurse) => CreateInstruction(
                    "hasKey",
                    recurse,
                    env,
                    step.Argument is P { OperatorName: "eq" } p
                        ? (object)p.Value
                        : step.Argument))
                .Override<HasPredicateStep>((step, env, overridden, recurse) =>
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
                .Override<HasTraversalStep>((step, env, overridden, recurse) => CreateInstruction("has", recurse, env, step.Key, step.Traversal))
                .Override<HasLabelStep>((step, env, overridden, recurse) => CreateInstruction("hasLabel", recurse, env, step.Labels))
                .Override<HasNotStep>((step, env, overridden, recurse) => CreateInstruction("hasNot", recurse, env, step.Key))
                .Override<HasValueStep>((step, env, overridden, recurse) => CreateInstruction(
                    "hasValue",
                    recurse,
                    env,
                    step.Argument is P { OperatorName: "eq" } p
                        ? (object)p.Value
                        : step.Argument))
                .Override<IdentityStep>((step, env, overridden, recurse) => CreateInstruction("identity"))
                .Override<IdStep>((step, env, overridden, recurse) => CreateInstruction("id"))
                .Override<IGremlinQueryBase>((query, env, overridden, recurse) => recurse.Serialize(query.ToTraversal(), env))
                .Override<InjectStep>((step, env, overridden, recurse) => CreateInstruction("inject", recurse, env, step.Elements))
                .Override<InEStep>((step, env, overridden, recurse) => CreateInstruction("inE", recurse, env, step.Labels))
                .Override<InStep>((step, env, overridden, recurse) => CreateInstruction("in", recurse, env, step.Labels))
                .Override<InVStep>((step, env, overridden, recurse) => CreateInstruction("inV"))
                .Override<IsStep>((step, env, overridden, recurse) => CreateInstruction(
                    "is",
                    recurse,
                    env,
                    step.Predicate.OperatorName == "eq"
                        ? (object)step.Predicate.Value
                        : step.Predicate))
                .Override<Key>((key, env, overridden, recurse) => recurse.Serialize(key.RawKey, env))
                .Override<KeyStep>((step, env, overridden, recurse) => CreateInstruction("key"))
                .Override<LabelStep>((step, env, overridden, recurse) => CreateInstruction("label"))
                .Override<LimitStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("limit", recurse, env, step.Scope, step.Count)
                    : CreateInstruction("limit", recurse, env, step.Count))
                .Override<LocalStep>((step, env, overridden, recurse) => CreateInstruction("local", recurse, env, step.Traversal))
                .Override<MaxStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("max", recurse, env, step.Scope)
                    : CreateInstruction("max"))
                .Override<MatchStep>((step, env, overridden, recurse) => CreateInstruction("match", recurse, env, step.Traversals))
                .Override<MapStep>((step, env, overridden, recurse) => CreateInstruction("map", recurse, env, step.Traversal))
                .Override<MeanStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("mean", recurse, env, step.Scope)
                    : CreateInstruction("mean"))
                .Override<MinStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("min", recurse, env, step.Scope)
                    : CreateInstruction("min"))
                .Override<NoneStep>((step, env, overridden, recurse) => CreateInstruction("none"))
                .Override<NotStep>((step, env, overridden, recurse) => CreateInstruction("not", recurse, env, step.Traversal))
                .Override<OptionalStep>((step, env, overridden, recurse) => CreateInstruction("optional", recurse, env, step.Traversal))
                .Override<OptionTraversalStep>((step, env, overridden, recurse) => CreateInstruction("option", recurse, env, step.Guard ?? Pick.None, step.OptionTraversal))
                .Override<OrderStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("order", recurse, env, step.Scope)
                    : CreateInstruction("order"))
                .Override<OrderStep.ByLambdaStep>((step, env, overridden, recurse) => CreateInstruction("by", recurse, env, step.Lambda))
                .Override<OrderStep.ByMemberStep>((step, env, overridden, recurse) => CreateInstruction("by", recurse, env, step.Key, step.Order))
                .Override<OrderStep.ByTraversalStep>((step, env, overridden, recurse) => CreateInstruction("by", recurse, env, step.Traversal, step.Order))
                .Override<OrStep>((step, env, overridden, recurse) => CreateInstruction("or", recurse, env, step.Traversals))
                .Override<OutStep>((step, env, overridden, recurse) => CreateInstruction("out", recurse, env, step.Labels))
                .Override<OutEStep>((step, env, overridden, recurse) => CreateInstruction("outE", recurse, env, step.Labels))
                .Override<OutVStep>((step, env, overridden, recurse) => CreateInstruction("outV"))
                .Override<OtherVStep>((step, env, overridden, recurse) => CreateInstruction("otherV"))
                .Override<P>((p, env, overridden, recurse) =>
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
                .Override<PathStep>((step, env, overridden, recurse) => CreateInstruction("path"))
                .Override<ProfileStep>((step, env, overridden, recurse) => CreateInstruction("profile"))
                .Override<PropertiesStep>((step, env, overridden, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
                .Override<PropertyStep.ByKeyStep>((step, env, overridden, recurse) =>
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
                .Override<ProjectStep>((step, env, overridden, recurse) => CreateInstruction("project", recurse, env, step.Projections))
                .Override<ProjectStep.ByTraversalStep>((step, env, overridden, recurse) =>
                {
                    var traversal = step.Traversal;

                    if (traversal.Count == 1 && traversal[0] is LocalStep localStep)
                        traversal = localStep.Traversal;

                    return CreateInstruction("by", recurse, env, traversal);
                })
                .Override<ProjectStep.ByKeyStep>((step, env, overridden, recurse) => CreateInstruction("by", recurse, env, step.Key))
                .Override<RangeStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("range", recurse, env, step.Scope, step.Lower, step.Upper)
                    : CreateInstruction("range", recurse, env, step.Lower, step.Upper))
                .Override<RepeatStep>((step, env, overridden, recurse) => CreateInstruction("repeat", recurse, env, step.Traversal))
                .Override<SelectStep>((step, env, overridden, recurse) => CreateInstruction("select", recurse, env, step.StepLabels))
                .Override<SelectKeysStep>((step, env, overridden, recurse) => CreateInstruction("select", recurse, env, step.Keys))
                .Override<SideEffectStep>((step, env, overridden, recurse) => CreateInstruction("sideEffect", recurse, env, step.Traversal))
                .Override<SimplePathStep>((step, env, overridden, recurse) => CreateInstruction("simplePath"))
                .Override<SkipStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("skip", recurse, env, step.Scope, step.Count)
                    : CreateInstruction("skip", recurse, env, step.Count))
                .Override<Step>((step, env, overridden, recurse) => Array.Empty<Instruction>())
                .Override<Traversal>((traversal, env, overridden, recurse) =>
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

                    if (steps.Count == 0)
                        steps = IdentitySteps;

                    Add(steps);

                    return byteCode;
                })
                .Override<SumStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("sum", recurse, env, step.Scope)
                    : CreateInstruction("sum"))
                .Override<TextP>((textP, env, overridden, recurse) => textP)
                .Override<TailStep>((step, env, overridden, recurse) => step.Scope.Equals(Scope.Local)
                    ? CreateInstruction("tail", recurse, env, step.Scope, step.Count)
                    : CreateInstruction("tail", recurse, env, step.Count))
                .Override<TimesStep>((step, env, overridden, recurse) => CreateInstruction("times", recurse, env, step.Count))
                .Override<Type>((type, env, overridden, recurse) => type)
                .Override<UnfoldStep>((step, env, overridden, recurse) => CreateInstruction("unfold"))
                .Override<UnionStep>((step, env, overridden, recurse) => CreateInstruction("union", recurse, env, step.Traversals))
                .Override<UntilStep>((step, env, overridden, recurse) => CreateInstruction("until", recurse, env, step.Traversal))
                .Override<ValueStep>((step, env, overridden, recurse) => CreateInstruction("value"))
                .Override<ValueMapStep>((step, env, overridden, recurse) => CreateInstruction("valueMap", recurse, env, step.Keys))
                .Override<ValuesStep>((step, env, overridden, recurse) => CreateInstruction("values", recurse, env, step.Keys))
                .Override<VStep>((step, env, overridden, recurse) => CreateInstruction("V", recurse, env, step.Ids))
                .Override<WhereTraversalStep>((step, env, overridden, recurse) => CreateInstruction("where", recurse, env, step.Traversal))
                .Override<WithStrategiesStep>((step, env, overridden, recurse) => CreateInstruction("withStrategies", recurse, env, step.Traversal))
                .Override<WithoutStrategiesStep>((step, env, overridden, recurse) => CreateInstruction("withoutStrategies", recurse, env, step.StrategyTypes))
                .Override<WithSideEffectStep>((step, env, overridden, recurse) => CreateInstruction("withSideEffect", recurse, env, step.Label, step.Value))
                .Override<WherePredicateStep>((step, env, overridden, recurse) => CreateInstruction("where", recurse, env, step.Predicate))
                .Override<WherePredicateStep.ByMemberStep>((step, env, overridden, recurse) => step.Key != null
                    ? CreateInstruction("by", recurse, env, step.Key)
                    : CreateInstruction("by"))
                .Override<WhereStepLabelAndPredicateStep>((step, env, overridden, recurse) => CreateInstruction("where", recurse, env, step.StepLabel, step.Predicate));
        }

        private static Instruction CreateInstruction(string name)
        {
            return SimpleInstructions.GetOrAdd(
                name,
                closure => new Instruction(closure));
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
                .Where(x => x != null)
                .Select(x => x!)
                .ToArray();

            return new Instruction(name, data);
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters)
        {
            if (parameters.Length == 0)
                return CreateInstruction(name);

            var data = parameters
                .Select(x => recurse.Serialize(x, env))
                .Where(x => x != null)
                .Select(x => x!)
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
