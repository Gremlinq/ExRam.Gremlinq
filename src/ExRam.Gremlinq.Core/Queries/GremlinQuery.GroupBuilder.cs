// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections.Generic;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class GroupBuilder<TKey, TValue> :
            IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>,
            IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>,
            IGroupBuilderWithKeyAndValue<TKey, TValue>
        {
            private readonly MultiContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> _continuationBuilder;

            public GroupBuilder(ContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder) : this(continuationBuilder.ToMulti())
            {

            }

            private GroupBuilder(MultiContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder)
            {
                _continuationBuilder = continuationBuilder;
            }

            IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewKey> IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByKey<TNewKey>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewKey>> keySelector)
            {
                return new GroupBuilder<TNewKey, object>(
                    _continuationBuilder
                        .With(keySelector));
            }

            IGroupBuilderWithKeyAndValue<TKey, TNewValue> IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>.ByValue<TNewValue>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewValue>> valueSelector)
            {
                return new GroupBuilder<TKey, TNewValue>(
                    _continuationBuilder
                        .With(valueSelector));
            }

            IValueGremlinQuery<IDictionary<TKey, object>> IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>.Build() => Build<TKey, object>();

            IValueGremlinQuery<IDictionary<TKey, TValue>> IGroupBuilderWithKeyAndValue<TKey, TValue>.Build() => Build<TKey, TValue>();

            private IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> Build<TNewKey, TNewValue>()
            {
                return _continuationBuilder
                    .Build(static (builder, traversals) =>
                    {
                        var keyTraversal = traversals[0];
                        var maybeValueTraversal = traversals.Length > 1
                            ? traversals[1]
                            : default(Traversal?);

                        builder = builder
                            .AddStep(GroupStep.Instance)
                            .AddStep(new GroupStep.ByTraversalStep(keyTraversal));

                        if (maybeValueTraversal is { } valueTraversal)
                        {
                            builder = builder
                                .AddStep(new GroupStep.ByTraversalStep(valueTraversal));
                        }

                        return builder
                            .WithNewProjection(
                                static (projection, state) => projection
                                    .Group(
                                        state.keyTraversal.Projection,
                                        state.maybeValueTraversal?.Projection ?? projection),
                                (keyTraversal, maybeValueTraversal))
                            .Build<IValueGremlinQuery<IDictionary<TNewKey, TNewValue>>>();
                    });
            }
        }
    }
}
