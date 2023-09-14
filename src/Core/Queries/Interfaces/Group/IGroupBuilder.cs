namespace ExRam.Gremlinq.Core
{
    public interface IGroupBuilder<TSourceElement, out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        IGroupBuilderWithKey<TSourceElement, TSourceQuery, TKey> ByKey<TKey>(Func<TSourceQuery, IGremlinQueryBase<TKey>> keySelector);
    }

    public interface IGroupBuilderWithKey<TSourceElement, out TSourceQuery, TKey>
        where TSourceQuery : IGremlinQueryBase
    {
        IGroupBuilderWithKeyAndValue<TKey, TValue> ByValue<TValue>(Func<TSourceQuery, IGremlinQueryBase<TValue>> valueSelector);

        IValueGremlinQuery<IDictionary<TKey, TSourceElement[]>> Build();
    }

    public interface IGroupBuilderWithKeyAndValue<TKey, TValue>
    {
        IValueGremlinQuery<IDictionary<TKey, TValue>> Build();
    }
}
