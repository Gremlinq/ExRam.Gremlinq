using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using ExRam.Gremlinq.Core.Steps;

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

            public virtual StepStack AddStep<TStep>(StepStack steps, TStep step, IGremlinQueryEnvironment environment) where TStep : Step
            {
                return TryGetAddHandler(typeof(TStep), step.GetType()) is Func<StepStack, TStep, IGremlinQueryEnvironment, IAddStepHandler, StepStack> del
                    ? del(steps, step, environment, this)
                    : steps.Push(step);
            }

            public virtual IAddStepHandler Override<TStep>(Func<StepStack, TStep, IGremlinQueryEnvironment, Func<StepStack, TStep, IGremlinQueryEnvironment, IAddStepHandler, StepStack>, IAddStepHandler, StepStack> addStepHandler) where TStep : Step
            {
                return new AddStepHandlerImpl(
                    _dict.SetItem(
                        typeof(TStep),
                        TryGetAddHandler(typeof(TStep), typeof(TStep)) is Func<StepStack, TStep, IGremlinQueryEnvironment, IAddStepHandler, StepStack> existingAddHandler
                            ? (steps, step, env, _, recurse) => addStepHandler(steps, step, env, existingAddHandler, recurse)
                            : addStepHandler));
            }

            private Delegate? TryGetAddHandler(Type staticType, Type actualType)
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

            private static Func<StepStack, TStatic, IGremlinQueryEnvironment, IAddStepHandler, StepStack> CreateFunc<TStatic, TEffective>(Func<StepStack, TEffective, IGremlinQueryEnvironment, Func<StepStack, TEffective, IGremlinQueryEnvironment, IAddStepHandler, StepStack>, IAddStepHandler, StepStack> del)
                where TEffective : Step
                where TStatic : Step
            {
                return (steps, step, environment, recurse) => del(steps, (TEffective)(Step)step!, environment, static (steps, step, _, _) => steps.Push(step), recurse);
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
            public override StepStack AddStep<TStep>(StepStack steps, TStep step, IGremlinQueryEnvironment environment) => steps.Push(step);
        }

        public static readonly IAddStepHandler Empty = new EmptyAddStepHandler();

        public static readonly IAddStepHandler Default = Empty
            .Override<AsStep>(static (steps, step, env, overridden, recurse) => steps.PeekOrDefault() is AsStep asStep && ReferenceEquals(asStep.StepLabel, step.StepLabel)
                ? steps
                : overridden(steps, step, env, recurse))
            .Override<HasLabelStep>(static (steps, step, env, overridden, recurse) => steps.PeekOrDefault() is HasLabelStep hasLabelStep
                ? steps
                    .Pop()
                    .Push(new HasLabelStep(step.Labels.Intersect(hasLabelStep.Labels).ToImmutableArray()))
                : overridden(steps, step, env, recurse))
            .Override<HasPredicateStep>(static (steps, step, env, overridden, recurse) =>
            {
                if (steps.PeekOrDefault() is HasPredicateStep hasStep && hasStep.Key == step.Key)
                {
                    var newPredicate = step.Predicate;

                    newPredicate = hasStep.Predicate is { } otherPredicate
                        ? otherPredicate.And(newPredicate)
                        : newPredicate;

                    return steps
                        .Pop()
                        .Push(new HasPredicateStep(hasStep.Key, newPredicate));
                }

                return overridden(steps, step, env, recurse);
            })
            .Override<IdentityStep>(static (steps, step, env, overridden, recurse) => steps.PeekOrDefault() is IdentityStep
                ? steps
                : overridden(steps, step, env, recurse))
            .Override<WithoutStrategiesStep>(static (steps, step, env, overridden, recurse) => (steps.PeekOrDefault() is WithoutStrategiesStep withoutStrategies)
                ? steps
                    .Pop()
                    .Push(new WithoutStrategiesStep(withoutStrategies.StrategyTypes.Concat(step.StrategyTypes).Distinct().ToImmutableArray()))
                : overridden(steps, step, env, recurse))
            .Override<SelectStepLabelStep>(static (steps, step, env, overridden, recurse) => steps.PeekOrDefault() is AsStep asStep && step.StepLabels.Length == 1 && ReferenceEquals(asStep.StepLabel, step.StepLabels[0])
                ? steps
                : overridden(steps, step, env, recurse))
            .Override<IsStep>(static (steps, step, env, overridden, recurse) => steps.PeekOrDefault() is IsStep isStep
                ? steps
                    .Pop()
                    .Push(new IsStep(isStep.Predicate.And(step.Predicate)))
                : overridden(steps, step, env, recurse))
            .Override<NoneStep>(static (steps, step, env, overridden, recurse) => steps.PeekOrDefault() is NoneStep
                ? steps
                : overridden(steps, step, env, recurse));
    }
}


