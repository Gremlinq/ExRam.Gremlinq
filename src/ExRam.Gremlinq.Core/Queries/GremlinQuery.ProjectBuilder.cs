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
            private readonly bool _enableEmptyProjectionValueProtection;
            private readonly MultiContinuationBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> _continuationBuilder;

            public ProjectBuilder(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery) : this(
                  sourceQuery.Continue().ToMulti(),
                  ImmutableList<string>.Empty,
                  sourceQuery.Environment.Options
                    .GetValue(GremlinqOption.EnableEmptyProjectionValueProtection))
            {
            }

            private ProjectBuilder(MultiContinuationBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder, ImmutableList<string> names, bool enableEmptyProjectionValueProtection)
            {
                _names = names;
                _continuationBuilder = continuationBuilder;
                _enableEmptyProjectionValueProtection = enableEmptyProjectionValueProtection;
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection, string? name = default)
            {
                return new(
                    _continuationBuilder
                        .With(__ => __
                            .Continue()
                            .With(projection)
                            .Build((builder, traversal) => builder
                                .AddStep(new ProjectStep.ByTraversalStep(traversal))
                                .Build())),
                    _names.Add(name ?? $"Item{_names.Count + 1}"),
                    _enableEmptyProjectionValueProtection);
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByExpression<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Expression projection, string? name = default)
            {
                return projection is LambdaExpression lambdaExpression && lambdaExpression.IsIdentityExpression()
                    ? ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(__ => __.Identity(), name)
                    : new(
                        _continuationBuilder
                            .With(__ => __
                                .AddStep(new ProjectStep.ByKeyStep(__.GetKey(projection)))),
                        _names.Add(name ?? $"Item{_names.Count + 1}"),
                        _enableEmptyProjectionValueProtection);
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
                    ? ByLambda<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(__ => __.Identity())
                    : projection.Body.StripConvert() is MemberExpression memberExpression
                        ? ByExpression<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(memberExpression, memberExpression.Member.Name)
                        : throw new ExpressionNotSupportedException(projection);
            }

            public TTargetQuery Build<TTargetQuery>() where TTargetQuery : IGremlinQueryBase
            {
                return _continuationBuilder!
                    .Build((builder, traversals) =>
                    {
                        var bySteps = traversals.Select(x => x.FirstOrDefault()).OfType<ProjectStep.ByStep>().ToArray();

                        ///// TODO: Remove this!!!!!
                        var zipped = _names
                            .Zip(bySteps, (name, step) => (name, step))
                            .OrderBy(t => t.name)
                            .ToArray();

                        var sortedNames = zipped
                            .Select(x => x.name)
                            .ToArray();

                        var sortedBySteps = zipped
                            .Select(x => x.step)
                            .ToArray();
                        /////////////

                        var projectStep = new ProjectStep(sortedNames.ToImmutableArray());


                        builder = builder
                            .AddStep(projectStep)
                            .WithNewProjection(_ => _.Project(
                                projectStep,
                                sortedBySteps));

                        foreach (var byStep in sortedBySteps)
                        {
                            var closureByStep = byStep;

                            if (_enableEmptyProjectionValueProtection)
                            {
                                var byTraversalStep = closureByStep
                                    .ToByTraversalStep();

                                closureByStep = new ProjectStep.ByTraversalStep(new Traversal(byTraversalStep.Traversal.Append(LimitStep.LimitGlobal1).Append(FoldStep.Instance), byTraversalStep.Traversal.Projection));
                            }

                            builder = builder
                                .AddStep(closureByStep);
                        }

                        if (_enableEmptyProjectionValueProtection)
                        {
                            foreach (var step in EmptyProjectionProtectionDecoratorSteps)
                            {
                                //TODO: Extension!
                                builder = builder
                                    .AddStep(step);
                            }
                        }

                        return builder
                            .Build<TTargetQuery>();
                    });
            }
        }

    }
}
