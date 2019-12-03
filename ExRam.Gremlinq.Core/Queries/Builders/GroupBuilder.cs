using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public static class GroupBuilder
    {
        private sealed class GroupBuilderImpl<TSourceQuery, TKey, TValue> :
            IGroupBuilder<TSourceQuery>,
            IGroupBuilderWithKey<TSourceQuery, TKey>,
            IGroupBuilderWithKeyAndValue<TSourceQuery, TKey, TValue>
            where TSourceQuery : IGremlinQuery
        {
            private readonly TSourceQuery _sourceQuery;

            public GroupBuilderImpl(TSourceQuery sourceQuery, IGremlinQuery<TKey>? keyQuery, IGremlinQuery<TValue>? valueQuery)
            {
                KeyQuery = keyQuery;
                ValueQuery = valueQuery;
                _sourceQuery = sourceQuery;
            }

            IGroupBuilderWithKey<TSourceQuery, TNewKey> IGroupBuilder<TSourceQuery>.ByKey<TNewKey>(Func<TSourceQuery, IGremlinQuery<TNewKey>> keySelector)
            {
                return new GroupBuilderImpl<TSourceQuery, TNewKey, Unit>(
                    _sourceQuery,
                    keySelector(_sourceQuery),
                    default);
            }

            IGroupBuilderWithKeyAndValue<TSourceQuery, TKey, TNewValue> IGroupBuilderWithKey<TSourceQuery, TKey>.ByValue<TNewValue>(Func<TSourceQuery, IGremlinQuery<TNewValue>> valueSelector)
            {
                return new GroupBuilderImpl<TSourceQuery, TKey, TNewValue>(
                    _sourceQuery,
                    KeyQuery,
                    valueSelector(_sourceQuery));
            }

            public IGremlinQuery<TKey> KeyQuery { get; }

            public IGremlinQuery<TValue> ValueQuery { get; }
        }

        public static IGroupBuilder<TSourceQuery> Create<TSourceQuery>(TSourceQuery query)
            where TSourceQuery : IGremlinQuery
        {
            return new GroupBuilderImpl<TSourceQuery, Unit, Unit>(query, default, default);
        }
    }
}
