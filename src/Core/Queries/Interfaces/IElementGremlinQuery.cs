using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for graph elements (vertices or edges) providing access to common element operations.
    /// </summary>
    public interface IElementGremlinQueryBase :
        IGremlinQueryBase
    {
        /// <summary>
        /// Casts the element query to a different result type.
        /// </summary>
        /// <typeparam name="TResult">The target result type.</typeparam>
        /// <returns>An element query with the specified result type.</returns>
        new IElementGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Gets the identifiers of the elements.
        /// </summary>
        /// <returns>A query that returns element identifiers.</returns>
        IGremlinQuery<object> Id();

        /// <summary>
        /// Gets the labels of the elements.
        /// </summary>
        /// <returns>A query that returns element labels.</returns>
        IGremlinQuery<string> Label();

        /// <summary>
        /// Gets all property values from the elements.
        /// </summary>
        /// <returns>A query that returns all property values.</returns>
        IGremlinQuery<object> Values();
        
        /// <summary>
        /// Gets all property values of a specific type from the elements.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <returns>A query that returns property values of the specified type.</returns>
        IGremlinQuery<TValue> Values<TValue>();

        /// <summary>
        /// Gets a map of property names to property values for each element.
        /// </summary>
        /// <returns>A query that returns dictionaries mapping property names to values.</returns>
        IMapGremlinQuery<IDictionary<string, object>> ValueMap();
        
        /// <summary>
        /// Gets a map of property names to property values of a specific type for each element.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <returns>A query that returns dictionaries mapping property names to typed values.</returns>
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>();
    }

    /// <summary>
    /// Represents a recursive element query with property setting capabilities.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IElementGremlinQueryBaseRec<TSelf> :
        IElementGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>
    {
        /// <summary>
        /// Sets a property on the elements.
        /// </summary>
        /// <param name="key">The property key (name).</param>
        /// <param name="value">The property value.</param>
        /// <returns>The query with the property set.</returns>
        TSelf Property(string key, object? value);
        
        /// <summary>
        /// Sets a property on the elements using a value from a traversal.
        /// </summary>
        /// <param name="key">The property key (name).</param>
        /// <param name="valueTransformation">A function that produces the property value.</param>
        /// <returns>The query with the property set.</returns>
        TSelf Property(string key, Func<TSelf, IGremlinQueryBase> valueTransformation);
    }

    /// <summary>
    /// Represents a strongly-typed element query with property access capabilities.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public interface IElementGremlinQueryBase<TElement> :
        IElementGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Updates the element with new property values.
        /// </summary>
        /// <param name="element">The element containing updated property values.</param>
        /// <returns>A query that returns the updated element.</returns>
        IElementGremlinQuery<TElement> Update(TElement element);

        /// <summary>
        /// Gets a map of property names to values for the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="keys">Expressions selecting the properties to include.</param>
        /// <returns>A query that returns dictionaries with the selected properties.</returns>
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params Expression<Func<TElement, TValue>>[] keys);
        
        /// <summary>
        /// Gets a map of property names to values for the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="keys">Expressions selecting the properties to include.</param>
        /// <returns>A query that returns dictionaries with the selected properties.</returns>
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params ReadOnlySpan<Expression<Func<TElement, TValue>>> keys);

        /// <summary>
        /// Gets the values of the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TElement, TValue>>[] projections);
        
        /// <summary>
        /// Gets the values of the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TElement, TValue>>> projections);

        /// <summary>
        /// Gets the values of the specified array properties, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the array properties.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TElement, TValue[]>>[] projections);
        
        /// <summary>
        /// Gets the values of the specified array properties, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the array properties.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TElement, TValue[]>>> projections);
    }

    /// <summary>
    /// Represents a recursive strongly-typed element query with property filtering and setting capabilities.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IElementGremlinQueryBaseRec<TElement, TSelf> :
        IElementGremlinQueryBaseRec<TSelf>,
        IElementGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>
    {
        /// <summary>
        /// Filters elements based on a traversal applied to a projected property.
        /// </summary>
        /// <typeparam name="TProjection">The type of the projected property.</typeparam>
        /// <param name="projection">Expression selecting the property to filter on.</param>
        /// <param name="propertyTraversal">A traversal to apply to the projected property for filtering.</param>
        /// <returns>The filtered query.</returns>
        TSelf Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQueryBase> propertyTraversal);

        /// <summary>
        /// Sets a property value on the elements.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">Expression selecting the property to set.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The query with the property set.</returns>
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, TProjectedValue value);
        
        /// <summary>
        /// Sets a property value from a step label on the elements.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">Expression selecting the property to set.</param>
        /// <param name="stepLabel">The step label containing the value to set.</param>
        /// <returns>The query with the property set.</returns>
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, StepLabel<TProjectedValue> stepLabel);
        
        /// <summary>
        /// Sets a property value from a traversal on the elements.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">Expression selecting the property to set.</param>
        /// <param name="valueTraversal">A traversal that produces the value to set.</param>
        /// <returns>The query with the property set.</returns>
        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, Func<TSelf, IGremlinQueryBase<TProjectedValue>> valueTraversal);
    }

    /// <summary>
    /// Represents a query for strongly-typed graph elements with full element operations.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public interface IElementGremlinQuery<TElement> :
        IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>;
}
