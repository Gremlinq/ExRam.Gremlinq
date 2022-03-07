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

        IValueGremlinQueryBase<TKey> KeyQuery { get; }
    }

    public interface IGroupBuilderWithKeyAndValue<out TSourceQuery, TKey, TValue>
        where TSourceQuery : IGremlinQueryBase
    {
        IValueGremlinQueryBase<TKey> KeyQuery { get; }

        IValueGremlinQueryBase<TValue> ValueQuery { get; }
    }
}
