namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Builds group-by operations for graph traversals.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IGroupBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the key selector for grouping.
        /// </summary>
        /// <typeparam name="TKey">The type of the grouping key.</typeparam>
        /// <param name="keySelector">A traversal that produces the grouping key.</param>
        /// <returns>A group builder with the key specified.</returns>
        IGroupBuilderWithKey<TSourceQuery, TKey> ByKey<TKey>(Func<TSourceQuery, IGremlinQueryBase<TKey>> keySelector);
    }

    /// <summary>
    /// Represents a group builder with a key specified that can specify a value selector.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key.</typeparam>
    public interface IGroupBuilderWithKey<out TSourceQuery, TKey>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the value selector for grouping.
        /// </summary>
        /// <typeparam name="TValue">The type of the grouped values.</typeparam>
        /// <param name="valueSelector">A traversal that produces the values to group.</param>
        /// <returns>A group builder with both key and value specified.</returns>
        IGroupBuilderWithKeyAndValue<TKey, TValue> ByValue<TValue>(Func<TSourceQuery, IGremlinQueryBase<TValue>> valueSelector);
    }

    /// <summary>
    /// Represents a complete group builder that can be finalized.
    /// </summary>
    /// <typeparam name="TKey">The type of the grouping key.</typeparam>
    /// <typeparam name="TValue">The type of the grouped values.</typeparam>
    public interface IGroupBuilderWithKeyAndValue<TKey, TValue>
    {
        /// <summary>
        /// Builds and returns the grouped query.
        /// </summary>
        /// <returns>A map query that returns dictionaries with grouped results.</returns>
        IMapGremlinQuery<IDictionary<TKey, TValue>> Build();
    }
}
