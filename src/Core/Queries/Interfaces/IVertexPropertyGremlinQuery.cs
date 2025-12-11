using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for vertex properties, which can have their own properties (meta-properties).
    /// </summary>
    public interface IVertexPropertyGremlinQueryBase : IElementGremlinQueryBase
    {
        /// <summary>
        /// Downcasts the query to a general element query.
        /// </summary>
        /// <returns>A general element query.</returns>
        new IElementGremlinQuery<object> Lower();

        /// <summary>
        /// Gets meta-properties (properties of the vertex property) with the specified keys.
        /// </summary>
        /// <param name="keys">The keys of the meta-properties to retrieve.</param>
        /// <returns>A query for the meta-properties.</returns>
        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);
        
        /// <summary>
        /// Gets meta-properties (properties of the vertex property) with the specified keys.
        /// </summary>
        /// <param name="keys">The keys of the meta-properties to retrieve.</param>
        /// <returns>A query for the meta-properties.</returns>
        IPropertyGremlinQuery<Property<object>> Properties(params ReadOnlySpan<string> keys);

        /// <summary>
        /// Gets the values of the vertex properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <returns>A query that returns the property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>();

        /// <summary>
        /// Gets the values of meta-properties with the specified keys.
        /// </summary>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="keys">The keys of the meta-properties.</param>
        /// <returns>A query that returns the meta-property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params string[] keys);
        
        /// <summary>
        /// Gets the values of meta-properties with the specified keys.
        /// </summary>
        /// <typeparam name="TValue">The type of the values.</typeparam>
        /// <param name="keys">The keys of the meta-properties.</param>
        /// <returns>A query that returns the meta-property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<string> keys);

        /// <summary>
        /// Gets the values of meta-properties with the specified keys.
        /// </summary>
        /// <param name="keys">The keys of the meta-properties.</param>
        /// <returns>A query that returns the meta-property values.</returns>
        IGremlinQuery<object> Values(params string[] keys);
        
        /// <summary>
        /// Gets the values of meta-properties with the specified keys.
        /// </summary>
        /// <param name="keys">The keys of the meta-properties.</param>
        /// <returns>A query that returns the meta-property values.</returns>
        IGremlinQuery<object> Values(params ReadOnlySpan<string> keys);

        /// <summary>
        /// Gets a map of meta-property names to values.
        /// </summary>
        /// <typeparam name="TValue">The type of the meta-property values.</typeparam>
        /// <returns>A query that returns dictionaries mapping meta-property names to values.</returns>
        new IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>();

        /// <summary>
        /// Gets a map of meta-property names to values for the specified keys.
        /// </summary>
        /// <typeparam name="TValue">The type of the meta-property values.</typeparam>
        /// <param name="keys">The keys of the meta-properties to include.</param>
        /// <returns>A query that returns dictionaries with the selected meta-properties.</returns>
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params string[] keys);
        
        /// <summary>
        /// Gets a map of meta-property names to values for the specified keys.
        /// </summary>
        /// <typeparam name="TValue">The type of the meta-property values.</typeparam>
        /// <param name="keys">The keys of the meta-properties to include.</param>
        /// <returns>A query that returns dictionaries with the selected meta-properties.</returns>
        IMapGremlinQuery<IDictionary<string, TValue>> ValueMap<TValue>(params ReadOnlySpan<string> keys);

        /// <summary>
        /// Gets a map of meta-property names to values for the specified keys.
        /// </summary>
        /// <param name="keys">The keys of the meta-properties to include.</param>
        /// <returns>A query that returns dictionaries with the selected meta-properties.</returns>
        IMapGremlinQuery<IDictionary<string, object>> ValueMap(params string[] keys);
        
        /// <summary>
        /// Gets a map of meta-property names to values for the specified keys.
        /// </summary>
        /// <param name="keys">The keys of the meta-properties to include.</param>
        /// <returns>A query that returns dictionaries with the selected meta-properties.</returns>
        IMapGremlinQuery<IDictionary<string, object>> ValueMap(params ReadOnlySpan<string> keys);
    }

    /// <summary>
    /// Represents a strongly-typed query for vertex properties.
    /// </summary>
    /// <typeparam name="TProperty">The vertex property type.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        /// <summary>
        /// Downcasts the query to a general element query.
        /// </summary>
        /// <returns>A general element query for the property type.</returns>
        new IElementGremlinQuery<TProperty> Lower();

        /// <summary>
        /// Converts the query to explicitly include metadata type information.
        /// </summary>
        /// <typeparam name="TMeta">The type of the meta-properties.</typeparam>
        /// <returns>A vertex property query with metadata type.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Meta<TMeta>();

        /// <summary>
        /// Gets meta-properties of the specified value type with the given keys.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="keys">The keys of the meta-properties to retrieve.</param>
        /// <returns>A query for the meta-properties.</returns>
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params string[] keys);
        
        /// <summary>
        /// Gets meta-properties of the specified value type with the given keys.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="keys">The keys of the meta-properties to retrieve.</param>
        /// <returns>A query for the meta-properties.</returns>
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params ReadOnlySpan<string> keys);

        /// <summary>
        /// Gets the value of the vertex property.
        /// </summary>
        /// <returns>A query that returns the property value.</returns>
        IGremlinQuery<TValue> Value();
    }

    /// <summary>
    /// Represents a query for strongly-typed vertex properties with full operations.
    /// </summary>
    /// <typeparam name="TProperty">The vertex property type.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    public interface IVertexPropertyGremlinQuery<TProperty, TValue> :
        IVertexPropertyGremlinQueryBase<TProperty, TValue>,
        IElementGremlinQueryBaseRec<TProperty, IVertexPropertyGremlinQuery<TProperty, TValue>>;

    /// <summary>
    /// Represents a strongly-typed query for vertex properties with metadata.
    /// </summary>
    /// <typeparam name="TProperty">The vertex property type.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <typeparam name="TMeta">The type of the meta-properties (properties of the property).</typeparam>
    public interface IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase,
        IElementGremlinQueryBase<TProperty>
    {
        /// <summary>
        /// Downcasts the query to a general element query.
        /// </summary>
        /// <returns>A general element query for the property type.</returns>
        new IElementGremlinQuery<TProperty> Lower();

        /// <summary>
        /// Gets meta-properties for the specified projections.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="projections">Expressions selecting the meta-properties.</param>
        /// <returns>A query for the selected meta-properties.</returns>
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        
        /// <summary>
        /// Gets meta-properties for the specified projections.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="projections">Expressions selecting the meta-properties.</param>
        /// <returns>A query for the selected meta-properties.</returns>
        IPropertyGremlinQuery<Property<TMetaValue>> Properties<TMetaValue>(params ReadOnlySpan<Expression<Func<TMeta, TMetaValue>>> projections);

        /// <summary>
        /// Sets a meta-property on the vertex property.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property value.</typeparam>
        /// <param name="projection">Expression selecting the meta-property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The query with the meta-property set.</returns>
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property<TMetaValue>(Expression<Func<TMeta, TMetaValue>> projection, TMetaValue value);

        /// <summary>
        /// Gets the value of the vertex property.
        /// </summary>
        /// <returns>A query that returns the property value.</returns>
        IGremlinQuery<TValue> Value();

        /// <summary>
        /// Gets the values of the specified meta-properties.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="projections">Expressions selecting the meta-properties.</param>
        /// <returns>A query that returns the meta-property values.</returns>
        IGremlinQuery<TMetaValue> Values<TMetaValue>(params Expression<Func<TMeta, TMetaValue>>[] projections);
        
        /// <summary>
        /// Gets the values of the specified meta-properties.
        /// </summary>
        /// <typeparam name="TMetaValue">The type of the meta-property values.</typeparam>
        /// <param name="projections">Expressions selecting the meta-properties.</param>
        /// <returns>A query that returns the meta-property values.</returns>
        IGremlinQuery<TMetaValue> Values<TMetaValue>(params ReadOnlySpan<Expression<Func<TMeta, TMetaValue>>> projections);

        /// <summary>
        /// Gets a map of all meta-property names to values.
        /// </summary>
        /// <returns>A query that returns the metadata.</returns>
        new IGremlinQuery<TMeta> ValueMap();

        /// <summary>
        /// Filters vertex properties based on a predicate.
        /// </summary>
        /// <param name="predicate">The filter predicate.</param>
        /// <returns>The filtered query.</returns>
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<VertexProperty<TValue, TMeta>, bool>> predicate);
    }

    /// <summary>
    /// Represents a query for strongly-typed vertex properties with metadata and full operations.
    /// </summary>
    /// <typeparam name="TProperty">The vertex property type.</typeparam>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <typeparam name="TMeta">The type of the meta-properties.</typeparam>
    public interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> :
        IVertexPropertyGremlinQueryBase<TProperty, TValue, TMeta>,
        IElementGremlinQueryBaseRec<TProperty, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>;
}
