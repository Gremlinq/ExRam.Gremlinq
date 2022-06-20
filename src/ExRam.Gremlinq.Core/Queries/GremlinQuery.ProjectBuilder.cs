// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed partial class ProjectBuilder<TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>,
            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>
        {
            private readonly ImmutableList<string> _names;
            private readonly Traversal? _emptyProjectionProtectionDecoratorSteps;
            private readonly MultiContinuationBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> _continuationBuilder;

            public ProjectBuilder(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery) : this(
                sourceQuery.Continue().ToMulti(),
                ImmutableList<string>.Empty,
                sourceQuery.Environment.Options.GetValue(GremlinqOption.EnableEmptyProjectionValueProtection)
                    ? sourceQuery.Environment.Options.GetValue(GremlinqOption.EmptyProjectionProtectionDecoratorSteps)
                    : default(Traversal?))
            {
            }

            private ProjectBuilder(
                MultiContinuationBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder,
                ImmutableList<string> names,
                Traversal? emptyProjectionProtectionDecoratorSteps)
            {
                _names = names;
                _continuationBuilder = continuationBuilder;
                _emptyProjectionProtectionDecoratorSteps = emptyProjectionProtectionDecoratorSteps;
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection, string? name = default)
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
                    _names.Add(name ?? $"Item{_names.Count + 1}"),
                    _emptyProjectionProtectionDecoratorSteps);
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByExpression<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Expression projection, string? name = default)
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
                        _names.Add(name ?? $"Item{_names.Count + 1}"),
                        _emptyProjectionProtectionDecoratorSteps);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.ToTuple()
            {
                return this;
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.ToDynamic()
            {
                return this;
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(string name, Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(string name, Expression<Func<TProjectElement, object>> projection)
            {
                return ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(Expression<Func<TProjectElement, object>> projection)
            {
                return projection.IsIdentityExpression()
                    ? ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(static __ => __.Identity())
                    : projection.Body.StripConvert() is MemberExpression memberExpression
                        ? ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(memberExpression, memberExpression.Member.Name)
                        : throw new ExpressionNotSupportedException(projection);
            }

            public TTargetQuery Build<TTargetQuery>() where TTargetQuery : IGremlinQueryBase
            {
                return _continuationBuilder
                    .Build(
                        static (builder, traversals, state) =>
                        {
                            var (names, emptyProjectionProtectionDecoratorSteps) = state;

                            var projectStep = new ProjectStep(names.ToImmutableArray());
                            var bySteps = traversals.Select(static x => x.FirstOrDefault()).OfType<ProjectStep.ByStep>().ToArray();

                            builder = builder
                                .AddStep(projectStep)
                                .WithNewProjection(
                                    static (projection, tuple) => projection.Project(tuple.projectStep, tuple.bySteps),
                                    (projectStep, bySteps));

                            foreach (var byStep in bySteps)
                            {
                                var closureByStep = byStep;

                                if (emptyProjectionProtectionDecoratorSteps is not null)
                                {
                                    var byTraversalStep = closureByStep
                                        .ToByTraversalStep();

                                    closureByStep = new ProjectStep.ByTraversalStep(byTraversalStep.Traversal
                                        .Append(LimitStep.LimitGlobal1)
                                        .Append(FoldStep.Instance)
                                        .ToTraversal()
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
