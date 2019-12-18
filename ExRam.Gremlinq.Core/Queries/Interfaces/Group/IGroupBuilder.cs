using System;

namespace ExRam.Gremlinq.Core
{
    public interface IGroupBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        IGroupBuilderWithKey<TSourceQuery, TKey> ByKey<TKey>(Func<TSourceQuery, IGremlinQueryBase<TKey>> keySelector);
    }

    public interface IGroupBuilderWithKey<out TSourceQuery, TKey>
        where TSourceQuery : IGremlinQueryBase
    {
        IGroupBuilderWithKeyAndValue<TSourceQuery, TKey, TValue> ByValue<TValue>(Func<TSourceQuery, IGremlinQueryBase<TValue>> valueSelector);

        IGremlinQueryBase<TKey> KeyQuery { get; }
    }

    public interface IGroupBuilderWithKeyAndValue<out TSourceQuery, TKey, TValue> : IGroupBuilderWithKey<TSourceQuery, TKey>
        where TSourceQuery : IGremlinQueryBase
    {
        IGremlinQueryBase<TValue> ValueQuery { get; }
    }
}
