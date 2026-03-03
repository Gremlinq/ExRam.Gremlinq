namespace ExRam.Gremlinq.Core
{
    /// <summary>Builder interface for constructing a <c>group()</c> step with key and value selectors.</summary>
    public interface IGroupBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the key selector for the group.
        /// Corresponds to the Gremlin <c>by()</c> key modulator on a <c>group()</c> step.
        /// </summary>
        /// <typeparam name="TKey">The type of the group key.</typeparam>
        /// <param name="keySelector">The traversal that selects the key.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
        IGroupBuilderWithKey<TSourceQuery, TKey> ByKey<TKey>(Func<TSourceQuery, IGremlinQueryBase<TKey>> keySelector);
    }

    /// <summary>A group builder with a key selector set, ready for a value selector.</summary>
    public interface IGroupBuilderWithKey<out TSourceQuery, TKey>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the value selector for the group.
        /// Corresponds to the Gremlin <c>by()</c> value modulator on a <c>group()</c> step.
        /// </summary>
        /// <typeparam name="TValue">The type of the group value.</typeparam>
        /// <param name="valueSelector">The traversal that selects the value.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#group-step">Reference Documentation - Group Step</seealso>
        IGroupBuilderWithKeyAndValue<TKey, TValue> ByValue<TValue>(Func<TSourceQuery, IGremlinQueryBase<TValue>> valueSelector);
    }

    /// <summary>A terminal group builder with both key and value selectors set.</summary>
    public interface IGroupBuilderWithKeyAndValue<TKey, TValue>
    {
        /// <summary>
        /// Builds and returns the group query.
        /// </summary>
        IMapGremlinQuery<IDictionary<TKey, TValue>> Build();
    }
}
