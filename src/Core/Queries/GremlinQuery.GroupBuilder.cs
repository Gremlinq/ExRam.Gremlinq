// ReSharper disable ArrangeThisQualifier
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4>
    {
        private sealed class GroupBuilder<TKey, TValue> :
            IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>,
            IGroupBuilderWithKey<GremlinQuery<T1, T2, T3, T4>, TKey>,
            IGroupBuilderWithKeyAndValue<TKey, TValue>
        {
            private readonly Traversal _keyTraversal;
            private readonly Traversal _valueTraversal;
            private readonly GremlinQuery<T1, T2, T3, T4> _outerQuery;

            public GroupBuilder(GremlinQuery<T1, T2, T3, T4> outerQuery) : this(outerQuery, Traversal.Empty, Traversal.Empty)
            {

            }

            private GroupBuilder(GremlinQuery<T1, T2, T3, T4> outerQuery, Traversal keyTraversal, Traversal valueTraversal)
            {
                _outerQuery = outerQuery;
                _keyTraversal = keyTraversal;
                _valueTraversal = valueTraversal;
            }

            IGroupBuilderWithKey<GremlinQuery<T1, T2, T3, T4>, TNewKey> IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>.ByKey<TNewKey>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<TNewKey>> keySelector)
            {
                ArgumentNullException.ThrowIfNull(keySelector);

                return new GroupBuilder<TNewKey, object>(
                    _outerQuery,
                    _outerQuery
                        .Continue()
                        .With(keySelector)
                        .Build(static (_, traversal) => traversal),
                    _valueTraversal);
            }

            IGroupBuilderWithKeyAndValue<TKey, TNewValue> IGroupBuilderWithKey<GremlinQuery<T1, T2, T3, T4>, TKey>.ByValue<TNewValue>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<TNewValue>> valueSelector)
            {
                ArgumentNullException.ThrowIfNull(valueSelector);

                return new GroupBuilder<TKey, TNewValue>(
                    _outerQuery,
                    _keyTraversal,
                    _outerQuery
                        .Continue()
                        .With(valueSelector)
                        .Build(static (_, traversal) => traversal));
            }

            IMapGremlinQuery<IDictionary<TKey, TValue>> IGroupBuilderWithKeyAndValue<TKey, TValue>.Build() => _outerQuery
                .Continue()
                .Build(
                    static (builder, tuple) =>
                    {
                        var (keyTraversal, valueTraversal) = tuple;
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
                            .BuildAs<IMapGremlinQuery<IDictionary<TKey, TValue>>>();
                    },
                    (_keyTraversal, _valueTraversal));
        }
    }
}
