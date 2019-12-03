using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGroupBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQuery
    {
        IGroupBuilderWithKey<TSourceQuery, TKey> ByKey<TKey>(Func<TSourceQuery, IGremlinQuery<TKey>> keySelector);
    }

    public interface IGroupBuilderWithKey<out TSourceQuery, TKey>
        where TSourceQuery : IGremlinQuery
    {
        IGroupBuilderWithKeyAndValue<TSourceQuery, TKey, TValue> ByValue<TValue>(Func<TSourceQuery, IGremlinQuery<TValue>> valueSelector);

        IGremlinQuery<TKey> KeyQuery { get; }
    }

    public interface IGroupBuilderWithKeyAndValue<out TSourceQuery, TKey, TValue> : IGroupBuilderWithKey<TSourceQuery, TKey>
        where TSourceQuery : IGremlinQuery
    {
        IGremlinQuery<TValue> ValueQuery { get; }
    }
}
