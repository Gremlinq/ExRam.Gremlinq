// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed partial class ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>,
            IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>,
            IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TItem1>
        {
            private readonly FastImmutableList<string> _names;
            private readonly Traversal? _emptyProjectionProtectionDecoratorSteps;
            private readonly MultiContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> _continuationBuilder;

            public ProjectBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery) : this(
                sourceQuery.Continue().ToMulti(),
                FastImmutableList<string>.Empty,
                sourceQuery.Environment.Options.GetValue(GremlinqOption.EnableEmptyProjectionValueProtection)
                    ? sourceQuery.Environment.Options.GetValue(GremlinqOption.EmptyProjectionProtectionDecoratorSteps)
                    : default(Traversal?))
            {
            }

            private ProjectBuilder(
                MultiContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder,
                FastImmutableList<string> names,
                Traversal? emptyProjectionProtectionDecoratorSteps)
            {
                _names = names;
                _continuationBuilder = continuationBuilder;
                _emptyProjectionProtectionDecoratorSteps = emptyProjectionProtectionDecoratorSteps;
            }

            IProjectTupleBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement> IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.ToTuple()
            {
                return this;
            }

            IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement> IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.ToDynamic()
            {
                return this;
            }

            IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TTargetType> IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.To<TTargetType>()
            {
                return new ProjectBuilder<TTargetType, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(_continuationBuilder, _names, _emptyProjectionProtectionDecoratorSteps);
            }

            IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement> IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement> IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.By(string name, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement> IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.By(string name, Expression<Func<TElement, object>> projection)
            {
                return ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement> IProjectDynamicBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>.By(Expression<Func<TElement, object>> projection)
            {
                return projection.IsIdentityExpression()
                    ? ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(static __ => __.Identity())
                    : projection.Body.StripConvert() is MemberExpression memberExpression
                        ? ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(memberExpression, memberExpression.Member.Name)
                        : throw new ExpressionNotSupportedException(projection);
            }

            IValueGremlinQuery<TItem1> IProjectTypeResult<TItem1>.Build() => Build<IValueGremlinQuery<TItem1>>();

            IValueGremlinQuery<dynamic> IProjectDynamicResult.Build() => Build<IValueGremlinQuery<dynamic>>();

            IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TItem1> IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TItem1>.By<TSourceProperty, TTargetProperty>(Expression<Func<TItem1, TTargetProperty>> targetExpression, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TSourceProperty>> projection)
            {
                return By(
                    targetExpression,
                    static (@this, memberName, projection) => @this.ByLambda<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(projection, memberName),
                    projection);
            }

            IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TItem1> IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TItem1>.By<TSourceProperty, TTargetProperty>(Expression<Func<TItem1, TTargetProperty>> targetExpression, Expression<Func<TElement, TSourceProperty>> projection)
            {
                return By(
                    targetExpression,
                    static (@this, memberName, projection) => @this.ByExpression<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>(projection, memberName),
                    projection);
            }

            private ProjectBuilder<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection, string? name = default)
            {
                return new(
                    _continuationBuilder
                        .With(
                            static (__, projection) => __
                                .Continue()
                                .With(projection)
                                .Build(static (builder, traversal) => builder
                                    .AddStep(new ProjectStep.ByTraversalStep(traversal))
                                    .Build()),
                            projection),
                    _names.Push(name ?? $"Item{_names.Count + 1}"),
                    _emptyProjectionProtectionDecoratorSteps);
            }

            private ProjectBuilder<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByExpression<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Expression projection, string? name = default)
            {
                return projection is LambdaExpression lambdaExpression && lambdaExpression.IsIdentityExpression()
                    ? ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(static __ => __.Identity(), name)
                    : new(
                        _continuationBuilder
                            .With(
                                static (__, projection) => __
                                    .Continue()
                                    .Build(
                                        static (builder, key) => builder
                                            .AddStep(new ProjectStep.ByKeyStep(key))
                                            .Build(),
                                        __.GetKey(projection)),
                                projection),
                        _names.Push(name ?? $"Item{_names.Count + 1}"),
                        _emptyProjectionProtectionDecoratorSteps);
            }

            private IProjectTypeBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement, TItem1> By<TTargetProperty, TState>(Expression<Func<TItem1, TTargetProperty>> targetExpression, Func<ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>, string, TState, ProjectBuilder<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16>> transformation, TState state)
            {
                return targetExpression.Body is MemberExpression memberExpression
                    ? transformation(this, memberExpression.Member.Name, state)
                    : throw new ExpressionNotSupportedException(targetExpression);
            }

            private TTargetQuery Build<TTargetQuery>() where TTargetQuery : IGremlinQueryBase
            {
                return _continuationBuilder
                    .Build(
                        static (builder, traversals, state) =>
                        {
                            var (names, emptyProjectionProtectionDecoratorSteps) = state;

                            var projectStep = new ProjectStep(names.ToImmutableArray());
                            var bySteps = new ProjectStep.ByStep[traversals.Length];

                            for (var i = 0; i < traversals.Length; i++)
                            {
                                bySteps[i] = (ProjectStep.ByStep)traversals[i][0]!;
                            }

                            builder = builder
                                .AddStep(projectStep)
                                .WithNewProjection(
                                    static (projection, tuple) => projection.Project(tuple.projectStep, tuple.bySteps),
                                    (projectStep, bySteps));

                            for (var i = 0; i < bySteps.Length; i++)
                            {
                                var closureByStep = bySteps[i];

                                if (emptyProjectionProtectionDecoratorSteps is not null)
                                {
                                    var byTraversalStep = closureByStep
                                        .ToByTraversalStep();

                                    closureByStep = new ProjectStep.ByTraversalStep(Traversal
                                        .Create(
                                            byTraversalStep.Traversal.Count + 2,
                                            byTraversalStep,
                                            static (steps, byTraversalStep) =>
                                            {
                                                steps[^2] = LimitStep.LimitGlobal1;
                                                steps[^1] = FoldStep.Instance;

                                                byTraversalStep.Traversal.Steps
                                                    .AsSpan()
                                                    .CopyTo(steps);
                                            })
                                        .WithProjection(byTraversalStep.Traversal.Projection));
                                }

                                builder = builder
                                    .AddStep(closureByStep);
                            }

                            if (emptyProjectionProtectionDecoratorSteps is not null)
                            {
                                builder = builder
                                    .AddSteps(emptyProjectionProtectionDecoratorSteps);
                            }

                            return builder
                                .Build<TTargetQuery>();
                        },
                        (_names, _emptyProjectionProtectionDecoratorSteps));
            }
        }
    }
}
