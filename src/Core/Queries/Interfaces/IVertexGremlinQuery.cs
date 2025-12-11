using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for graph vertices with navigation and property access capabilities.
    /// </summary>
    public interface IVertexGremlinQueryBase :
        IEdgeOrVertexGremlinQueryBase
    {
        /// <summary>
        /// Navigates to adjacent vertices connected by any edge in both directions.
        /// </summary>
        /// <returns>A query for the adjacent vertices.</returns>
        IVertexGremlinQuery<object> Both();
        
        /// <summary>
        /// Navigates to adjacent vertices connected by edges of the specified type in both directions.
        /// </summary>
        /// <typeparam name="TEdge">The edge type to traverse.</typeparam>
        /// <returns>A query for the adjacent vertices.</returns>
        IVertexGremlinQuery<object> Both<TEdge>();

        /// <summary>
        /// Navigates to all incident edges in both directions.
        /// </summary>
        /// <returns>A query for the incident edges.</returns>
        IEdgeGremlinQuery<object> BothE();
        
        /// <summary>
        /// Navigates to incident edges of the specified type in both directions.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <returns>A query for the incident edges.</returns>
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();

        /// <summary>
        /// Casts the vertex query to a different result type.
        /// </summary>
        /// <typeparam name="TResult">The target result type.</typeparam>
        /// <returns>A vertex query with the specified result type.</returns>
        new IVertexGremlinQuery<TResult> Cast<TResult>();
        
        /// <summary>
        /// Navigates to vertices connected by incoming edges.
        /// </summary>
        /// <returns>A query for the incoming vertices.</returns>
        IVertexGremlinQuery<object> In();
        
        /// <summary>
        /// Navigates to vertices connected by incoming edges of the specified type.
        /// </summary>
        /// <typeparam name="TEdge">The edge type to traverse.</typeparam>
        /// <returns>A query for the incoming vertices.</returns>
        IVertexGremlinQuery<object> In<TEdge>();

        /// <summary>
        /// Navigates to incoming edges.
        /// </summary>
        /// <returns>A query for the incoming edges.</returns>
        IEdgeGremlinQuery<object> InE();
        
        /// <summary>
        /// Navigates to incoming edges of the specified type.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <returns>A query for the incoming edges.</returns>
        IEdgeGremlinQuery<TEdge> InE<TEdge>();

        /// <summary>
        /// Downcasts the query to a less specific edge-or-vertex query type.
        /// </summary>
        /// <returns>A general edge-or-vertex query.</returns>
        new IEdgeOrVertexGremlinQuery<object> Lower();

        /// <summary>
        /// Filters vertices to only those of the specified type.
        /// </summary>
        /// <typeparam name="TTarget">The target vertex type.</typeparam>
        /// <returns>A query for vertices of the specified type.</returns>
        IVertexGremlinQuery<TTarget> OfType<TTarget>();

        /// <summary>
        /// Navigates to vertices connected by outgoing edges.
        /// </summary>
        /// <returns>A query for the outgoing vertices.</returns>
        IVertexGremlinQuery<object> Out();
        
        /// <summary>
        /// Navigates to vertices connected by outgoing edges of the specified type.
        /// </summary>
        /// <typeparam name="TEdge">The edge type to traverse.</typeparam>
        /// <returns>A query for the outgoing vertices.</returns>
        IVertexGremlinQuery<object> Out<TEdge>();

        /// <summary>
        /// Navigates to outgoing edges.
        /// </summary>
        /// <returns>A query for the outgoing edges.</returns>
        IEdgeGremlinQuery<object> OutE();
        
        /// <summary>
        /// Navigates to outgoing edges of the specified type.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <returns>A query for the outgoing edges.</returns>
        IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }

    /// <summary>
    /// Represents a strongly-typed query for graph vertices with full vertex operations.
    /// </summary>
    /// <typeparam name="TVertex">The vertex type.</typeparam>
    public interface IVertexGremlinQueryBase<TVertex> :
        IVertexGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TVertex>
    {
        /// <summary>
        /// Updates the vertex with new property values.
        /// </summary>
        /// <param name="element">The vertex containing updated property values.</param>
        /// <returns>A query that returns the updated vertex.</returns>
        new IVertexGremlinQuery<TVertex> Update(TVertex element);

        /// <summary>
        /// Adds an edge from this vertex.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <param name="edge">The edge to add.</param>
        /// <returns>A query for the added edge.</returns>
        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        
        /// <summary>
        /// Adds an edge of the specified type from this vertex.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <returns>A query for the added edge.</returns>
        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        /// <summary>
        /// Navigates to incoming edges of the specified type.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <returns>A query for the incoming edges with vertex context.</returns>
        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();

        /// <summary>
        /// Downcasts the query to a less specific edge-or-vertex query type.
        /// </summary>
        /// <returns>A general edge-or-vertex query for the vertex type.</returns>
        new IEdgeOrVertexGremlinQuery<TVertex> Lower();

        /// <summary>
        /// Navigates to outgoing edges of the specified type.
        /// </summary>
        /// <typeparam name="TEdge">The edge type.</typeparam>
        /// <returns>A query for the outgoing edges with vertex context.</returns>
        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        /// <summary>
        /// Gets all vertex properties.
        /// </summary>
        /// <returns>A query for all vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties();
        
        /// <summary>
        /// Gets all vertex properties of the specified value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <returns>A query for vertex properties with the specified value type.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>();

        /// <summary>
        /// Gets vertex properties for the specified projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        
        /// <summary>
        /// Gets vertex properties for the specified projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue>>> projections);

        /// <summary>
        /// Gets vertex properties for the specified property projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        
        /// <summary>
        /// Gets vertex properties for the specified property projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>>>> projections);

        /// <summary>
        /// Gets vertex properties for the specified projections.
        /// </summary>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);
        
        /// <summary>
        /// Gets vertex properties for the specified projections.
        /// </summary>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<object>>>> projections);

        /// <summary>
        /// Gets vertex properties for the specified array projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting array properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        
        /// <summary>
        /// Gets vertex properties for the specified array projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting array properties.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue[]>>> projections);

        /// <summary>
        /// Gets vertex properties for the specified array property projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting vertex property arrays.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        
        /// <summary>
        /// Gets vertex properties for the specified array property projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting vertex property arrays.</param>
        /// <returns>A query for the selected vertex properties.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>[]>>> projections);

        /// <summary>
        /// Gets vertex properties with metadata for the specified projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties with metadata.</param>
        /// <returns>A query for the selected vertex properties with metadata.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);
        
        /// <summary>
        /// Gets vertex properties with metadata for the specified projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties with metadata.</param>
        /// <returns>A query for the selected vertex properties with metadata.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>> projections);

        /// <summary>
        /// Gets vertex properties with metadata for the specified array projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting vertex property arrays with metadata.</param>
        /// <returns>A query for the selected vertex properties with metadata.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);
        
        /// <summary>
        /// Gets vertex properties with metadata for the specified array projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting vertex property arrays with metadata.</param>
        /// <returns>A query for the selected vertex properties with metadata.</returns>
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>> projections);

        /// <summary>
        /// Gets the values of the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        
        /// <summary>
        /// Gets the values of the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue>>> projections);

        /// <summary>
        /// Gets the values of the specified array properties, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the array properties.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        
        /// <summary>
        /// Gets the values of the specified array properties, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the array properties.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue[]>>> projections);

        /// <summary>
        /// Gets the values from the specified vertex properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified vertex properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>>>> projections);

        /// <summary>
        /// Gets the values from the specified vertex properties.
        /// </summary>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<object> Values(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified vertex properties.
        /// </summary>
        /// <param name="projections">Expressions selecting the vertex properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<object> Values(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<object>>>> projections);

        /// <summary>
        /// Gets the values from the specified vertex property arrays, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the vertex property arrays.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified vertex property arrays, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the vertex property arrays.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>[]>>> projections);

        /// <summary>
        /// Gets the values from the specified vertex properties with metadata.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties with metadata.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified vertex properties with metadata.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting the vertex properties with metadata.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>> projections);

        /// <summary>
        /// Gets the values from the specified vertex property arrays with metadata, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting the vertex property arrays with metadata.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        IGremlinQuery<TValue> Values<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified vertex property arrays with metadata, flattening the arrays.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <typeparam name="TMeta">The type of the property metadata.</typeparam>
        /// <param name="projections">Expressions selecting the vertex property arrays with metadata.</param>
        /// <returns>A query that returns the flattened property values.</returns>
        IGremlinQuery<TValue> Values<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>> projections);
    }

    /// <summary>
    /// Represents a query for strongly-typed graph vertices with full vertex operations.
    /// </summary>
    /// <typeparam name="TVertex">The vertex type.</typeparam>
    public interface IVertexGremlinQuery<TVertex> :
        IVertexGremlinQueryBase<TVertex>,
        IEdgeOrVertexGremlinQueryBaseRec<TVertex, IVertexGremlinQuery<TVertex>>
    {
        /// <summary>
        /// Sets a multi-valued property on the vertices by adding a value to an array property.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property values.</typeparam>
        /// <param name="projection">Expression selecting the array property.</param>
        /// <param name="value">The value to add to the array property.</param>
        /// <returns>The query with the property value added.</returns>
        IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }
}
