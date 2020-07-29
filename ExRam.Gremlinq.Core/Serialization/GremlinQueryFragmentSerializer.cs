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
    public static class GremlinQueryFragmentSerializer
    {
        private sealed class GremlinQueryFragmentSerializerImpl : IGremlinQueryFragmentSerializer
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new ConcurrentDictionary<(Type staticType, Type actualType), Delegate?>();

            public GremlinQueryFragmentSerializerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public object Serialize<TFragment>(TFragment fragment, IGremlinQueryEnvironment gremlinQueryEnvironment)
            {
                return TryGetSerializer(typeof(TFragment), fragment!.GetType()) is Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object> del
                    ? del(fragment, gremlinQueryEnvironment, this)
                    : fragment;
            }

            public IGremlinQueryFragmentSerializer Override<TFragment>(GremlinQueryFragmentSerializerDelegate<TFragment> serializer)
            {
                return new GremlinQueryFragmentSerializerImpl(
                    _dict.SetItem(
                        typeof(TFragment),
                        TryGetSerializer(typeof(TFragment), typeof(TFragment)) is Func<TFragment, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer, object> existingFragmentSerializer
                            ? (fragment, env, overridden, recurse) => serializer(fragment, env, existingFragmentSerializer, recurse)
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
                                //return (TStatic fragment, IGremlinQueryEnvironment environment, IGremlinQueryFragmentSerializer recurse) => del((TEffective)fragment, environment, (TEffective _, IGremlinQueryEnvironment, IGremlinQueryFragmentSerializer) => _, recurse);

                                var effectiveType = del.GetType().GetGenericArguments()[0];
                                var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var recurseParameter = Expression.Parameter(typeof(IGremlinQueryFragmentSerializer));

                                var argument3Parameter1 = Expression.Parameter(effectiveType);
                                var argument3Parameter2 = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var argument3Parameter3 = Expression.Parameter(typeof(IGremlinQueryFragmentSerializer));

                                var fragmentParameterExpression = Expression.Parameter(staticType);

                                var staticTypeFunc = typeof(Func<,,,>).MakeGenericType(
                                    staticType,
                                    environmentParameter.Type,
                                    recurseParameter.Type,
                                    typeof(object));

                                var effectiveTypeFunc = typeof(Func<,,,>).MakeGenericType(
                                    argument3Parameter1.Type,
                                    argument3Parameter2.Type,
                                    argument3Parameter3.Type,
                                    typeof(object));

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    Expression.Convert(
                                        fragmentParameterExpression,
                                        effectiveType),
                                    environmentParameter,
                                    Expression.Lambda(
                                        effectiveTypeFunc,
                                        Expression.Convert(argument3Parameter1, typeof(object)),
                                        argument3Parameter1,
                                        argument3Parameter2,
                                        argument3Parameter3),
                                    recurseParameter);

                                return Expression
                                    .Lambda(
                                        staticTypeFunc,
                                        retCall,
                                        fragmentParameterExpression,
                                        environmentParameter,
                                        recurseParameter)
                                    .Compile();
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
        }

        public static readonly IGremlinQueryFragmentSerializer Identity = new GremlinQueryFragmentSerializerImpl(ImmutableDictionary<Type, Delegate>.Empty);

        public static readonly IGremlinQueryFragmentSerializer Default = Identity.UseDefaultGremlinStepSerializationHandlers();

        private static readonly ImmutableArray<Step> IdentitySteps = ImmutableArray.Create((Step)IdentityStep.Instance);
        private static readonly ConcurrentDictionary<string, Instruction> SimpleInstructions = new ConcurrentDictionary<string, Instruction>();

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
                .Override<ChoosePredicateStep>((step, env, overridden, recurse) =>
                {
                    return step.ElseTraversal is { } elseTraversal
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
                            step.ThenTraversal);
                })
                .Override<ChooseTraversalStep>((step, env, overridden, recurse) =>
                {
                    return step.ElseTraversal is { } elseTraversal
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
                            step.ThenTraversal);
                })
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
                    step.Argument is P p && p.OperatorName == "eq"
                        ? p.Value
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
                    };

                    if (UnwindHasPredicateStep(step) is Step unwound && unwound != step)
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
                    step.Argument is P p && p.OperatorName == "eq"
                        ? p.Value
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
                        ? step.Predicate.Value
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
                .Override<NoneStep>((step, env, overridden, recurse) => recurse.Serialize(GremlinQueryEnvironment.NoneWorkaround, env))
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
                        !(p.Value is string) && p.Value is IEnumerable enumerable
                            ? enumerable
                                .Cast<object>()
                                .Select(x => recurse.Serialize(x, env))
                                .ToArray()
                            : recurse.Serialize(p.Value, env),
                        p.Other is { } other
                            ? recurse.Serialize(other, env) as P
                            : null);
                })
                .Override<ProfileStep>((step, env, overridden, recurse) => CreateInstruction("profile"))
                .Override<PropertiesStep>((step, env, overridden, recurse) => CreateInstruction("properties", recurse, env, step.Keys))
                .Override<PropertyStep>((step, env, overridden, recurse) =>
                {
                    static IEnumerable<object> GetPropertyStepArguments(PropertyStep propertyStep)
                    {
                        if (propertyStep.Cardinality != null && !T.Id.Equals(propertyStep.Key.RawKey))
                            yield return propertyStep.Cardinality;

                        yield return propertyStep.Key;
                        yield return propertyStep.Value;

                        foreach (var metaProperty in propertyStep.MetaProperties)
                        {
                            yield return metaProperty;
                        }
                    }

                    return (T.Id.Equals(step.Key.RawKey) && !Cardinality.Single.Equals(step.Cardinality ?? Cardinality.Single))
                        ? throw new NotSupportedException("Cannot have an id property on non-single cardinality.")
                        : CreateInstruction("property", recurse, env, GetPropertyStepArguments(step));
                })
                .Override<ProjectStep>((step, env, overridden, recurse) => CreateInstruction("project", recurse, env, step.Projections))
                .Override<ProjectStep.ByTraversalStep>((step, env, overridden, recurse) =>
                {
                    var traversal = step.Traversal;

                    if (traversal.Steps.Length == 1 && traversal.Steps[0] is LocalStep localStep)
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
                    var steps = traversal.Steps;

                    void Add(object obj)
                    {
                        switch (obj)
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

                    if (steps.Length == 0)
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
            return new Instruction(
                name,
                recurse.Serialize(parameter, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2)
        {
            return new Instruction(
                name,
                recurse.Serialize(parameter1, env),
                recurse.Serialize(parameter2, env));
        }

        private static Instruction CreateInstruction<TParam1, TParam2, TParam3>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam1 parameter1, TParam2 parameter2, TParam3 parameter3)
        {
            return new Instruction(
                name,
                recurse.Serialize(parameter1, env),
                recurse.Serialize(parameter2, env),
                recurse.Serialize(parameter3, env));
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, TParam[] parameters)
        {
            if (parameters.Length == 0)
                return CreateInstruction(name);

            var data = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                data[i] = recurse.Serialize(parameters[i], env);
            }

            return new Instruction(name, data);
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, ImmutableArray<TParam> parameters)
        {
            if (parameters.Length == 0)
                return CreateInstruction(name);

            var data = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                data[i] = recurse.Serialize(parameters[i], env);
            }

            return new Instruction(name, data);
        }

        private static Instruction CreateInstruction<TParam>(string name, IGremlinQueryFragmentSerializer recurse, IGremlinQueryEnvironment env, IEnumerable<TParam> parameters)
        {
            return new Instruction(
                name,
                parameters
                    .Select(x => recurse.Serialize(x, env))
                    .ToArray());
        }
    }
}
