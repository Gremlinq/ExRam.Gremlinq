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

            public IImmutableStack<Step> AddStep<TStep>(IImmutableStack<Step> steps, TStep step, IGremlinQueryEnvironment environment) where TStep : Step
            {
                return TryGetAddHandler(typeof(TStep), step.GetType()) is Func<IImmutableStack<Step>, TStep, IGremlinQueryEnvironment, IAddStepHandler, IImmutableStack<Step>> del
                    ? del(steps, step, environment, this)
                    : steps.Push(step);
            }

            public IAddStepHandler Override<TStep>(Func<IImmutableStack<Step>, TStep, IGremlinQueryEnvironment, Func<IImmutableStack<Step>, TStep, IGremlinQueryEnvironment, IAddStepHandler, IImmutableStack<Step>>, IAddStepHandler, IImmutableStack<Step>> addStepHandler) where TStep : Step
            {
                return new AddStepHandlerImpl(
                    _dict.SetItem(
                        typeof(TStep),
                        TryGetAddHandler(typeof(TStep), typeof(TStep)) is Func<IImmutableStack<Step>, TStep, IGremlinQueryEnvironment, IAddStepHandler, IImmutableStack<Step>> existingAddHandler
                            ? (steps, step, env, baseHandler, recurse) => addStepHandler(steps, step, env, existingAddHandler, recurse)
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
                                //return (IImmutableStack<Step> steps, TStatic step, IGremlinQueryEnvironment environment, IAddStepHandler recurse) => del(steps, (TEffective)step, env, (steps, TEffective step, IGremlinQueryEnvironment, IAddStepHandler) => _.Push(__), recurse);

                                var effectiveType = del.GetType().GetGenericArguments()[1];

                                var argument3Parameter1 = Expression.Parameter(typeof(IImmutableStack<Step>));
                                var argument3Parameter2 = Expression.Parameter(effectiveType);
                                var argument3Parameter3 = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var argument3Parameter4 = Expression.Parameter(typeof(IAddStepHandler));

                                var stepsParameterExpression = Expression.Parameter(typeof(IImmutableStack<Step>));
                                var stepParameterExpression = Expression.Parameter(staticType);
                                var environmentParameterExpression = Expression.Parameter(typeof(IGremlinQueryEnvironment));
                                var recurseParameterExpression = Expression.Parameter(typeof(IAddStepHandler));

                                var staticFuncType = typeof(Func<,,,,>).MakeGenericType(
                                    stepsParameterExpression.Type,
                                    staticType,
                                    environmentParameterExpression.Type,
                                    recurseParameterExpression.Type,
                                    typeof(IImmutableStack<Step>));

                                var overrideFuncType = typeof(Func<,,,,>).MakeGenericType(
                                    argument3Parameter1.Type,
                                    argument3Parameter2.Type,
                                    argument3Parameter3.Type,
                                    argument3Parameter4.Type,
                                    typeof(IImmutableStack<Step>));

                                var pushCall = Expression.Call(
                                    argument3Parameter1,
                                    typeof(IImmutableStack<Step>).GetMethod(nameof(IImmutableStack<Step>.Push))!,
                                    argument3Parameter2);

                                var retCall = Expression.Invoke(
                                    Expression.Constant(del),
                                    stepsParameterExpression,
                                    Expression.Convert(
                                        stepParameterExpression,
                                        effectiveType),
                                    environmentParameterExpression,
                                    Expression.Lambda(
                                        overrideFuncType,
                                        pushCall,
                                        argument3Parameter1,
                                        argument3Parameter2,
                                        argument3Parameter3,
                                        argument3Parameter4),
                                    recurseParameterExpression);

                                return Expression
                                    .Lambda(
                                        staticFuncType,
                                        retCall,
                                        stepsParameterExpression,
                                        stepParameterExpression,
                                        environmentParameterExpression,
                                        recurseParameterExpression)
                                    .Compile();
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
        }

        public static IAddStepHandler Empty = new AddStepHandlerImpl(ImmutableDictionary<Type, Delegate>.Empty);

        public static IAddStepHandler Default = Empty
            .Override<AsStep>((steps, step, env, overridden, recurse) => steps.PeekOrDefault() is AsStep asStep && ReferenceEquals(asStep.StepLabel, step.StepLabel)
                ? steps
                : overridden(steps, step, env, recurse))
            .Override<HasLabelStep>((steps, step, env, overridden, recurse) => steps.PeekOrDefault() is HasLabelStep hasLabelStep
                ? steps
                    .Pop()
                    .Push(new HasLabelStep(step.Labels.Intersect(hasLabelStep.Labels).ToImmutableArray()))
                : overridden(steps, step, env, recurse))
            .Override<HasPredicateStep>((steps, step, env, overridden, recurse) =>
            {
                if (steps.PeekOrDefault() is HasPredicateStep hasStep && hasStep.Key == step.Key)
                {
                    var newPredicate = step.Predicate;

                    if (newPredicate == null)
                        return steps;

                    newPredicate = hasStep.Predicate is { } otherPredicate
                        ? otherPredicate.And(newPredicate)
                        : newPredicate;

                    return steps
                        .Pop()
                        .Push(new HasPredicateStep(hasStep.Key, newPredicate));
                }

                return overridden(steps, step, env, recurse);
            })
            .Override<WithoutStrategiesStep>((steps, step, env, overridden, recurse) => (steps.PeekOrDefault() is WithoutStrategiesStep withoutStrategies)
                ? steps
                    .Pop()
                    .Push(new WithoutStrategiesStep(withoutStrategies.StrategyTypes.Concat(step.StrategyTypes).Distinct().ToImmutableArray()))
                : overridden(steps, step, env, recurse))
            .Override<SelectStep>((steps, step, env, overridden, recurse) => steps.PeekOrDefault() is AsStep asStep && step.StepLabels.Length == 1 && ReferenceEquals(asStep.StepLabel, step.StepLabels[0])
                ? steps
                : overridden(steps, step, env, recurse))
            .Override<IsStep>((steps, step, env, overridden, recurse) => steps.PeekOrDefault() is IsStep isStep
                ? steps
                    .Pop()
                    .Push(new IsStep(isStep.Predicate.And(step.Predicate)))
                : overridden(steps, step, env, recurse))
            .Override<NoneStep>((steps, step, env, overridden, recurse) => steps.PeekOrDefault() is NoneStep ? steps : overridden(steps, step, env, recurse));
    }
}


