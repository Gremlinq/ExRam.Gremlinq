namespace ExRam.Gremlinq.Core
{
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

    public interface IGroupBuilderWithKeyAndValue<TKey, TValue>
    {
        /// <summary>
        /// Builds and returns the group query.
        /// </summary>
        IMapGremlinQuery<IDictionary<TKey, TValue>> Build();
    }
}
