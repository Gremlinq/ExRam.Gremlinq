// ReSharper disable ArrangeThisQualifier
using System;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>
    {
        private sealed class GroupBuilder<TKey, TValue> :
            IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>,
            IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>,
            IGroupBuilderWithKeyAndValue<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey, TValue>
        {
            private readonly IValueGremlinQueryBase<TKey>? _keyQuery;
            private readonly IValueGremlinQueryBase<TValue>? _valueQuery;
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _sourceQuery;

            public GroupBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery, IValueGremlinQueryBase<TKey>? keyQuery = default, IValueGremlinQueryBase<TValue>? valueQuery = default)
            {
                _keyQuery = keyQuery;
                _valueQuery = valueQuery;
                _sourceQuery = sourceQuery;
            }

            IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewKey> IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByKey<TNewKey>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewKey>> keySelector)
            {
                return new GroupBuilder<TNewKey, object>(
                    _sourceQuery,
                    _sourceQuery
                        .Continue()
                        .With(keySelector)
                        .Build((_, keyQuery) => keyQuery
                            .AsAdmin()
                            .ChangeQueryType<IValueGremlinQueryBase<TNewKey>>()));
            }

            IGroupBuilderWithKeyAndValue<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey, TNewValue> IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>.ByValue<TNewValue>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewValue>> valueSelector)
            {
                return new GroupBuilder<TKey, TNewValue>(
                    _sourceQuery,
                    KeyQuery,
                    _sourceQuery
                        .Continue()
                        .With(valueSelector)
                        .Build((_, valueQuery) => valueQuery
                            .AsAdmin()
                            .ChangeQueryType<IValueGremlinQueryBase<TNewValue>>()));
            }

            public IValueGremlinQueryBase<TKey> KeyQuery
            {
                get => _keyQuery is { } keyQuery
                    ? keyQuery
                    : throw new InvalidOperationException();
            }

            public IValueGremlinQueryBase<TValue> ValueQuery
            {
                get => _valueQuery is { } valueQuery
                    ? valueQuery
                    : throw new InvalidOperationException();
            }
        }
    }
}
