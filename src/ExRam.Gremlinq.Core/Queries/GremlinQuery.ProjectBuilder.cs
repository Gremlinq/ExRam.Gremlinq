// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections.Immutable;
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
            private readonly GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _sourceQuery;

            public ProjectBuilder(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery) : this(sourceQuery, ImmutableDictionary<string, ProjectStep.ByStep>.Empty)
            {
            }

            private ProjectBuilder(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery, IImmutableDictionary<string, ProjectStep.ByStep> projections)
            {
                _sourceQuery = sourceQuery;
                Projections = projections;
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection, string? name = default)
            {
                return ByStep<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(new ProjectStep.ByTraversalStep(_sourceQuery.ContinueInner(projection, true).ToTraversal()), name);
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByExpression<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Expression projection, string? name = default)
            {
                return projection is LambdaExpression lambdaExpression && lambdaExpression.IsIdentityExpression()
                    ? ByLambda<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(__ => __.Identity(), name)
                    : ByStep<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(new ProjectStep.ByKeyStep(_sourceQuery.GetKey(projection)), name);
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> ByStep<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(ProjectStep.ByStep step, string? name = default)
            {
                return new(
                    _sourceQuery,
                    Projections.SetItem(name ?? $"Item{Projections.Count + 1}", step));
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

            public IImmutableDictionary<string, ProjectStep.ByStep> Projections { get; }
        }

    }
}
