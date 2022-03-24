using System;
using System.Collections.Generic;

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
        IGroupBuilderWithKeyAndValue<TKey, TValue> ByValue<TValue>(Func<TSourceQuery, IGremlinQueryBase<TValue>> valueSelector);

        IValueGremlinQuery<IDictionary<TKey, object>> Build();
    }

    public interface IGroupBuilderWithKeyAndValue<TKey, TValue>
    {
        IValueGremlinQuery<IDictionary<TKey, TValue>> Build();
    }
}
