using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for graph edges with navigation capabilities.
    /// </summary>
    public interface IEdgeGremlinQueryBase : IEdgeOrVertexGremlinQueryBase
    {
        /// <summary>
        /// Navigates to vertices at both ends of the edges.
        /// </summary>
        /// <returns>A query for the vertices connected by the edges.</returns>
        IVertexGremlinQuery<object> BothV();
        
        /// <summary>
        /// Navigates to vertices of a specific type at both ends of the edges.
        /// </summary>
        /// <typeparam name="TVertex">The vertex type.</typeparam>
        /// <returns>A query for the typed vertices connected by the edges.</returns>
        IVertexGremlinQuery<TVertex> BothV<TVertex>();

        /// <summary>
        /// Casts the edge query to a different result type.
        /// </summary>
        /// <typeparam name="TResult">The target result type.</typeparam>
        /// <returns>An edge query with the specified result type.</returns>
        new IEdgeGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Navigates to the incoming vertices (head/target vertices) of the edges.
        /// </summary>
        /// <returns>A query for the incoming vertices.</returns>
        IVertexGremlinQuery<object> InV();
        
        /// <summary>
        /// Navigates to the incoming vertices of a specific type.
        /// </summary>
        /// <typeparam name="TVertex">The vertex type.</typeparam>
        /// <returns>A query for the typed incoming vertices.</returns>
        IVertexGremlinQuery<TVertex> InV<TVertex>();

        /// <summary>
        /// Downcasts the query to a less specific edge-or-vertex query type.
        /// </summary>
        /// <returns>A general edge-or-vertex query.</returns>
        new IEdgeOrVertexGremlinQuery<object> Lower();

        /// <summary>
        /// Filters edges to only those of the specified type.
        /// </summary>
        /// <typeparam name="TTarget">The target edge type.</typeparam>
        /// <returns>A query for edges of the specified type.</returns>
        IEdgeGremlinQuery<TTarget> OfType<TTarget>();

        /// <summary>
        /// Navigates to the vertex at the other end of the edge from the current traversal context.
        /// </summary>
        /// <returns>A query for the other vertex.</returns>
        IVertexGremlinQuery<object> OtherV();
        
        /// <summary>
        /// Navigates to the vertex of a specific type at the other end of the edge.
        /// </summary>
        /// <typeparam name="TVertex">The vertex type.</typeparam>
        /// <returns>A query for the typed other vertex.</returns>
        IVertexGremlinQuery<TVertex> OtherV<TVertex>();

        /// <summary>
        /// Navigates to the outgoing vertices (tail/source vertices) of the edges.
        /// </summary>
        /// <returns>A query for the outgoing vertices.</returns>
        IVertexGremlinQuery<object> OutV();
        
        /// <summary>
        /// Navigates to the outgoing vertices of a specific type.
        /// </summary>
        /// <typeparam name="TVertex">The vertex type.</typeparam>
        /// <returns>A query for the typed outgoing vertices.</returns>
        IVertexGremlinQuery<TVertex> OutV<TVertex>();

        /// <summary>
        /// Gets all properties of the edges.
        /// </summary>
        /// <returns>A query for all edge properties.</returns>
        IPropertyGremlinQuery<Property<object>> Properties();
        
        /// <summary>
        /// Gets all properties of a specific value type.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <returns>A query for edge properties with the specified value type.</returns>
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>();
    }

    /// <summary>
    /// Represents a strongly-typed query for graph edges with property access and vertex navigation.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    public interface IEdgeGremlinQueryBase<TEdge> :
        IEdgeGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TEdge>
    {
        /// <summary>
        /// Updates the edge with new property values.
        /// </summary>
        /// <param name="element">The edge containing updated property values.</param>
        /// <returns>A query that returns the updated edge.</returns>
        new IEdgeGremlinQuery<TEdge> Update(TEdge element);

        /// <summary>
        /// Sets the outgoing (source) vertex of the edge using a traversal.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="fromVertexTraversal">A traversal that selects the outgoing vertex.</param>
        /// <returns>An outgoing edge query with vertex type information.</returns>
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);
        
        /// <summary>
        /// Sets the outgoing (source) vertex of the edge using a step label.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the outgoing vertex.</param>
        /// <returns>An outgoing edge query with vertex type information.</returns>
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        /// <summary>
        /// Downcasts the query to a less specific edge-or-vertex query type.
        /// </summary>
        /// <returns>A general edge-or-vertex query for the edge type.</returns>
        new IEdgeOrVertexGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Gets properties for the specified projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected properties.</returns>
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, TValue>>[] projections);
        
        /// <summary>
        /// Gets properties for the specified projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected properties.</returns>
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params ReadOnlySpan<Expression<Func<TEdge, TValue>>> projections);

        /// <summary>
        /// Gets properties for the specified property projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected properties.</returns>
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        
        /// <summary>
        /// Gets properties for the specified property projections.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected properties.</returns>
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params ReadOnlySpan<Expression<Func<TEdge, Property<TValue>>>> projections);

        /// <summary>
        /// Gets properties for the specified projections.
        /// </summary>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected properties.</returns>
        IPropertyGremlinQuery<Property<object>> Properties(params Expression<Func<TEdge, Property<object>>>[] projections);
        
        /// <summary>
        /// Gets properties for the specified projections.
        /// </summary>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query for the selected properties.</returns>
        IPropertyGremlinQuery<Property<object>> Properties(params ReadOnlySpan<Expression<Func<TEdge, Property<object>>>> projections);

        /// <summary>
        /// Sets the incoming (target) vertex of the edge using a traversal.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <param name="toVertexTraversal">A traversal that selects the incoming vertex.</param>
        /// <returns>An incoming edge query with vertex type information.</returns>
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
        
        /// <summary>
        /// Sets the incoming (target) vertex of the edge using a step label.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the incoming vertex.</param>
        /// <returns>An incoming edge query with vertex type information.</returns>
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        /// <summary>
        /// Gets the values of the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TEdge, TValue>>[] projections);
        
        /// <summary>
        /// Gets the values of the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TEdge, TValue>>> projections);

        /// <summary>
        /// Gets the values from the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified properties.
        /// </summary>
        /// <typeparam name="TValue">The type of the property values.</typeparam>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TEdge, Property<TValue>>>> projections);

        /// <summary>
        /// Gets the values from the specified properties.
        /// </summary>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<object> Values(params Expression<Func<TEdge, Property<object>>>[] projections);
        
        /// <summary>
        /// Gets the values from the specified properties.
        /// </summary>
        /// <param name="projections">Expressions selecting the properties.</param>
        /// <returns>A query that returns the property values.</returns>
        IGremlinQuery<object> Values(params ReadOnlySpan<Expression<Func<TEdge, Property<object>>>> projections);
    }

    /// <summary>
    /// Represents a recursive edge query.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IEdgeGremlinQueryBaseRec<TSelf> : IEdgeGremlinQueryBase, IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>
    /// Represents a recursive strongly-typed edge query.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IEdgeGremlinQueryBaseRec<TEdge, TSelf> :
        IEdgeGremlinQueryBaseRec<TSelf>,
        IEdgeGremlinQueryBase<TEdge>,
        IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IEdgeGremlinQueryBaseRec<TEdge, TSelf>;

    /// <summary>
    /// Represents a query for strongly-typed graph edges with full edge operations.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    public interface IEdgeGremlinQuery<TEdge> :
        IEdgeGremlinQueryBaseRec<TEdge, IEdgeGremlinQuery<TEdge>>;

    /// <summary>
    /// Represents a query for strongly-typed edges with known outgoing and incoming vertex types.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (source) vertex type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (target) vertex type.</typeparam>
    public interface IEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> :
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>
    {
        /// <summary>
        /// Removes vertex type information from the edge query.
        /// </summary>
        /// <returns>An edge query without vertex type constraints.</returns>
        new IEdgeGremlinQuery<TEdge> Lower();
    }

    /// <summary>
    /// Represents a query for strongly-typed edges with known outgoing and incoming vertex types and full edge operations.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (source) vertex type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (target) vertex type.</typeparam>
    public interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IOutEdgeGremlinQueryBaseRec<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IInEdgeGremlinQueryBaseRec<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>
    {
        /// <summary>
        /// Removes vertex type information from the edge query.
        /// </summary>
        /// <returns>An edge query without vertex type constraints.</returns>
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Converts the edge query to an incoming edge query, emphasizing the incoming vertex type.
        /// </summary>
        /// <returns>An incoming edge query.</returns>
        IInEdgeGremlinQuery<TEdge, TInVertex> AsInEdge();
        
        /// <summary>
        /// Converts the edge query to an outgoing edge query, emphasizing the outgoing vertex type.
        /// </summary>
        /// <returns>An outgoing edge query.</returns>
        IOutEdgeGremlinQuery<TEdge, TOutVertex> AsOutEdge();
    }
}
