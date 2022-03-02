// ReSharper disable ArrangeThisQualifier
using System;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class ChooseBuilder<TTargetQuery, TPickElement> :
            IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>,
            IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement>,
            IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TTargetQuery>
            where TTargetQuery : IGremlinQueryBase
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _targetQuery;
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _sourceQuery;

            public ChooseBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> targetQuery)
            {
                _sourceQuery = sourceQuery;
                _targetQuery = targetQuery;
            }

            public IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewPickElement> On<TNewPickElement>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewPickElement>> chooseTraversal)
            {
                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _sourceQuery,
                    _targetQuery.AddStep(new ChooseOptionTraversalStep(_sourceQuery.ContinueInner(chooseTraversal).ToTraversal())));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                var traversal = _sourceQuery
                    .ContinueInner(continuation)
                    .ToTraversal();

                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AddStep(
                        new OptionTraversalStep(element, traversal),
                        _ => _.Lowest(traversal.Projection)));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                var traversal = _sourceQuery
                    .ContinueInner(continuation)
                    .ToTraversal();

                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                   _targetQuery.AddStep(
                       new OptionTraversalStep(default, traversal),
                       _ => _.Lowest(traversal.Projection)));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery => _targetQuery.ChangeQueryType<TTargetQuery>();
        }
    }
}
