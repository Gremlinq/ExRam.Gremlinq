// ReSharper disable ArrangeThisQualifier
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class GroupBuilder<TKey, TValue> :
            IGroupBuilder<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>>,
            IGroupBuilderWithKey<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>,
            IGroupBuilderWithKeyAndValue<TKey, TValue>
        {
            private readonly MultiContinuationBuilder<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>> _continuationBuilder;

            public GroupBuilder(ContinuationBuilder<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder) : this(continuationBuilder.ToMulti())
            {

            }

            private GroupBuilder(MultiContinuationBuilder<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>> continuationBuilder)
            {
                _continuationBuilder = continuationBuilder;
            }

            public IGroupBuilderWithKey<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewKey> ByKey<TNewKey>(Func<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewKey>> keySelector)
            {
                return new GroupBuilder<TNewKey, object>(
                    _continuationBuilder
                        .With(keySelector));
            }

            public IGroupBuilderWithKeyAndValue<TKey, TNewValue> ByValue<TNewValue>(Func<GremlinQuery<T1, T2, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewValue>> valueSelector)
            {
                return new GroupBuilder<TKey, TNewValue>(
                    _continuationBuilder
                        .With(valueSelector));
            }

            IMapGremlinQuery<IDictionary<TKey, TValue>> IGroupBuilderWithKeyAndValue<TKey, TValue>.Build()
            {
                return _continuationBuilder
                    .Build(static (builder, traversals) =>
                    {
                        var keyTraversal = traversals[0];
                        var valueTraversal = traversals[1];
                        var valueTraversalIsSingleFoldStep = valueTraversal is [FoldStep];

                        builder = builder
                            .AddStep(GroupStep.Instance);

                        if (!keyTraversal.IsIdentity() || !valueTraversalIsSingleFoldStep)
                        {
                            builder = builder
                                .AddStep(new GroupStep.ByTraversalStep(keyTraversal));

                            if (!valueTraversalIsSingleFoldStep)
                            {
                                builder = builder
                                    .AddStep(new GroupStep.ByTraversalStep(valueTraversal));
                            }
                        }

                        return builder
                            .WithNewProjection(
                                static (projection, state) => projection
                                    .Group(
                                        state.keyTraversal.Projection,
                                        state.valueTraversal.Projection),
                                (keyTraversal, valueTraversal))
                            .Build<IMapGremlinQuery<IDictionary<TKey, TValue>>>();
                    });
            }
        }
    }
}
