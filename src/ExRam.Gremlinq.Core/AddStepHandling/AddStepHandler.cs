using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public static class AddStepHandler
    {
        private abstract class AddStepHandlerBase : IAddStepHandler
        {
            private static readonly MethodInfo CreateFuncMethod = typeof(AddStepHandlerBase).GetMethod(nameof(CreateFunc), BindingFlags.Static | BindingFlags.NonPublic)!;

            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new();
            
            protected AddStepHandlerBase() : this(ImmutableDictionary<Type, Delegate>.Empty)
            {
            }

            protected AddStepHandlerBase(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public virtual StepStack AddStep<TStep>(StepStack steps, TStep step, QuerySemantics? semantics, IGremlinQueryEnvironment environment) where TStep : Step
            {
                return TryGetAddHandler(typeof(TStep), step.GetType()) is Func<StepStack, TStep, QuerySemantics? , IGremlinQueryEnvironment, IAddStepHandler, StepStack> del
                    ? del(steps, step, semantics, environment, this)
                    : steps.Push(step, semantics);
            }

            public virtual IAddStepHandler Override<TStep>(Func<StepStack, TStep, QuerySemantics?, IGremlinQueryEnvironment, Func<StepStack, TStep, QuerySemantics?, IGremlinQueryEnvironment, IAddStepHandler, StepStack>, IAddStepHandler, StepStack> addStepHandler) where TStep : Step
            {
                return new AddStepHandlerImpl(
                    _dict.SetItem(
                        typeof(TStep),
                        TryGetAddHandler(typeof(TStep), typeof(TStep)) is Func<StepStack, TStep, QuerySemantics?, IGremlinQueryEnvironment, IAddStepHandler, StepStack> existingAddHandler
                            ? (steps, step, semantics, env, _, recurse) => addStepHandler(steps, step, semantics, env, existingAddHandler, recurse)
                            : addStepHandler));
            }

            private Delegate? TryGetAddHandler(Type staticType, Type actualType)
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
                                    .GetGenericArguments()[1];

                                var method = CreateFuncMethod
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
                return _dict.TryGetValue(actualType, out var ret)
                    ? ret
                    : actualType.BaseType is { } baseType && InnerLookup(baseType) is { } baseHandler
                        ? baseHandler
                        : null;
            }

            private static Func<StepStack, TStatic, QuerySemantics?, IGremlinQueryEnvironment, IAddStepHandler, StepStack> CreateFunc<TStatic, TEffective>(Func<StepStack, TEffective, QuerySemantics?, IGremlinQueryEnvironment, Func<StepStack, TEffective, QuerySemantics?, IGremlinQueryEnvironment, IAddStepHandler, StepStack>, IAddStepHandler, StepStack> del)
                where TEffective : Step
                where TStatic : Step
            {
                return (steps, step, semantics, environment, recurse) => del(steps, (TEffective)(Step)step!, semantics, environment, (steps, step, semantics, _, _) => semantics is { } s ? steps.Push(step, s) : steps.Push(step), recurse);
            }
        }

        private sealed class AddStepHandlerImpl : AddStepHandlerBase
        {
            public AddStepHandlerImpl(IImmutableDictionary<Type, Delegate> dict) : base(dict)
            {
            }
        }

        private sealed class EmptyAddStepHandler : AddStepHandlerBase
        {
            public override StepStack AddStep<TStep>(StepStack steps, TStep step, QuerySemantics? semantics, IGremlinQueryEnvironment environment) => steps.Push(step, semantics);
        }

        public static readonly IAddStepHandler Empty = new EmptyAddStepHandler();

        public static readonly IAddStepHandler Default = Empty
            .Override<AsStep>((steps, step, semantics, env, overridden, recurse) => steps.PeekOrDefault()?.Step is AsStep asStep && ReferenceEquals(asStep.StepLabel, step.StepLabel)
                ? steps
                : overridden(steps, step, semantics, env, recurse))
            .Override<HasLabelStep>((steps, step, semantics, env, overridden, recurse) => steps.PeekOrDefault()?.Step  is HasLabelStep hasLabelStep
                ? steps
                    .Pop()
                    .Push(new HasLabelStep(step.Labels.Intersect(hasLabelStep.Labels).ToImmutableArray()), semantics)
                : overridden(steps, step, semantics,env, recurse))
            .Override<HasPredicateStep>((steps, step, semantics, env, overridden, recurse) =>
            {
                if (steps.PeekOrDefault()?.Step is HasPredicateStep hasStep && hasStep.Key == step.Key)
                {
                    var newPredicate = step.Predicate;

                    newPredicate = hasStep.Predicate is { } otherPredicate
                        ? otherPredicate.And(newPredicate)
                        : newPredicate;

                    return steps
                        .Pop()
                        .Push(new HasPredicateStep(hasStep.Key, newPredicate), semantics);
                }

                return overridden(steps, step, semantics, env, recurse);
            })
            .Override<WithoutStrategiesStep>((steps, step, semantics, env, overridden, recurse) => (steps.PeekOrDefault()?.Step is WithoutStrategiesStep withoutStrategies)
                ? steps
                    .Pop()
                    .Push(new WithoutStrategiesStep(withoutStrategies.StrategyTypes.Concat(step.StrategyTypes).Distinct().ToImmutableArray()), semantics)
                : overridden(steps, step, semantics, env, recurse))
            .Override<SelectStep>((steps, step, semantics, env, overridden, recurse) => steps.PeekOrDefault()?.Step is AsStep asStep && step.StepLabels.Length == 1 && ReferenceEquals(asStep.StepLabel, step.StepLabels[0])
                ? steps
                : overridden(steps, step, semantics, env, recurse))
            .Override<IsStep>((steps, step, semantics, env, overridden, recurse) => steps.PeekOrDefault()?.Step is IsStep isStep
                ? steps
                    .Pop()
                    .Push(new IsStep(isStep.Predicate.And(step.Predicate)), semantics)
                : overridden(steps, step, semantics, env, recurse))
            .Override<NoneStep>((steps, step, semantics, env, overridden, recurse) => steps.PeekOrDefault()?.Step is NoneStep ? steps : overridden(steps, step, semantics, env, recurse));
    }
}


