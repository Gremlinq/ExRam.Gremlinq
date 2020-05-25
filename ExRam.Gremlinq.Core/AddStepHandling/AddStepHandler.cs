using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public static class AddStepHandler
    {
        private sealed class AddStepHandlerImpl : IAddStepHandler
        {
            private readonly IImmutableDictionary<Type, Delegate> _dict;
            private readonly ConcurrentDictionary<(Type staticType, Type actualType), Delegate?> _fastDict = new ConcurrentDictionary<(Type staticType, Type actualType), Delegate?>();

            public AddStepHandlerImpl(IImmutableDictionary<Type, Delegate> dict)
            {
                _dict = dict;
            }

            public IImmutableStack<Step> AddStep<TStep>(IImmutableStack<Step> steps, TStep step) where TStep : Step
            {
                return TryGetAddHandler(typeof(TStep), step.GetType()) is Func<IImmutableStack<Step>, TStep, IImmutableStack<Step>> del
                    ? del(steps, step)
                    : steps.Push(step);
            }

            public IAddStepHandler Override<TStep>(Func<IImmutableStack<Step>, TStep, IAddStepHandler, IImmutableStack<Step>> addStepHandler) where TStep : Step
            {
                return new AddStepHandlerImpl(
                    _dict.SetItem(
                        typeof(TStep),
                        addStepHandler));
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
                                //return (IImmutableStack<Step> steps, TStatic step) => del(steps, (TActualType)step, @this);

                                var effectiveType = del.GetType().GetGenericArguments()[1];
                                var stepsParameterExpression = Expression.Parameter(typeof(IImmutableStack<Step>));
                                var stepParameterExpression = Expression.Parameter(staticType);
                                var staticTypeFunc = typeof(Func<,,>).MakeGenericType(typeof(IImmutableStack<Step>), staticType, typeof(IImmutableStack<Step>));

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    stepsParameterExpression,
                                    Expression.Convert(
                                        stepParameterExpression,
                                        effectiveType),
                                    Expression.Constant(@this));

                                return Expression
                                    .Lambda(
                                        staticTypeFunc,
                                        retCall,
                                        stepsParameterExpression,
                                        stepParameterExpression)
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

                if (actualType.BaseType is { } baseType)
                {
                    if (InnerLookup(baseType) is { } baseSerializer)
                        return baseSerializer;
                }

                return null;
            }
        }

        public static IAddStepHandler Empty = new AddStepHandlerImpl(ImmutableDictionary<Type, Delegate>.Empty);

        public static IAddStepHandler Default = Empty
            .Override<HasLabelStep>((steps, step, recurse) => steps.PeekOrDefault() is HasLabelStep hasLabelStep
                ? steps
                    .Pop()
                    .Push(new HasLabelStep(step.Labels.Intersect(hasLabelStep.Labels).ToImmutableArray()))
                : steps.Push(step))
            .Override<HasPredicateStep>((steps, step, recurse) =>
            {
                if (steps.PeekOrDefault() is HasPredicateStep hasStep && hasStep.Key == step.Key)
                {
                    var newPredicate = step.Predicate;

                    if (newPredicate == null)
                        return steps;

                    steps = steps.Pop();
                    newPredicate = hasStep.Predicate is { } otherPredicate
                        ? otherPredicate.And(newPredicate)
                        : newPredicate;

                    return steps.Push(new HasPredicateStep(hasStep.Key, newPredicate));
                }

                return steps.Push(step);
            })
            .Override<WithoutStrategiesStep>((steps, step, recurse) =>
            {
                return (steps.PeekOrDefault() is WithoutStrategiesStep withoutStrategies)
                    ? steps.Pop().Push(new WithoutStrategiesStep(withoutStrategies.StrategyTypes.Concat(step.StrategyTypes).Distinct().ToImmutableArray()))
                    : steps.Push(step);
            })
            .Override<SelectStep>((steps, step, recurse) =>
            {
                return steps.PeekOrDefault() is AsStep asStep && step.StepLabels.Length == 1 && ReferenceEquals(asStep.StepLabel, step.StepLabels[0])
                    ? steps
                    : steps.Push(step);
            });
    }
}


