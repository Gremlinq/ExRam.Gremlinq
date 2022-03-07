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
            private readonly ContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> _continuation;

            public ChooseBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> query) : this(query.Continue(), query)
            {

            }

            private ChooseBuilder(ContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> continuation, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> targetQuery)
            {
                _continuation = continuation
                    .WithOuter(targetQuery!);
            }

            public IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewPickElement> On<TNewPickElement>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewPickElement>> chooseContinuation)
            {
                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _continuation,
                    _continuation
                        .With(chooseContinuation)
                        .Build((builder, traversal) => builder
                            .AddStep(new ChooseOptionTraversalStep(traversal))
                            .Build()));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _continuation,
                    _continuation
                        .With(continuation)
                        .Build((builder, traversal) => builder
                            .AddStep(new OptionTraversalStep(element, traversal))
                            .WithNewProjection(_ => _.Lowest(traversal.Projection))
                            .Build()));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _continuation,
                    _continuation
                        .With(continuation)
                        .Build((builder, traversal) => builder
                            .AddStep(new OptionTraversalStep(default, traversal))
                            .WithNewProjection(_ => _.Lowest(traversal.Projection))
                            .Build()));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery => _continuation.Build(builder => builder.Build<TTargetQuery>());
        }
    }
}
