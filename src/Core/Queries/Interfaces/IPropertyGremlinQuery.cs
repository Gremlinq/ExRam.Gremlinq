namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for graph element properties.
    /// </summary>
    public interface IPropertyGremlinQueryBase : IGremlinQueryBase
    {
        /// <summary>
        /// Casts the property query to a different result type.
        /// </summary>
        /// <typeparam name="TResult">The target result type.</typeparam>
        /// <returns>A property query with the specified result type.</returns>
        new IPropertyGremlinQuery<TResult> Cast<TResult>();
    }

    /// <summary>
    /// Represents a strongly-typed query for graph element properties.
    /// </summary>
    /// <typeparam name="TElement">The property type.</typeparam>
    public interface IPropertyGremlinQueryBase<TElement> :
        IPropertyGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Gets the keys (names) of the properties.
        /// </summary>
        /// <returns>A query that returns property keys.</returns>
        IGremlinQuery<string> Key();

        /// <summary>
        /// Gets the values of the properties.
        /// </summary>
        /// <returns>A query that returns property values as objects.</returns>
        IGremlinQuery<object> Value();
        
        /// <summary>
        /// Gets the values of the properties as a specific type.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <returns>A query that returns typed property values.</returns>
        IGremlinQuery<TValue> Value<TValue>();
    }

    /// <summary>
    /// Represents a query for strongly-typed graph element properties with full query capabilities.
    /// </summary>
    /// <typeparam name="TElement">The property type.</typeparam>
    public interface IPropertyGremlinQuery<TElement> :
        IPropertyGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>;
}
