namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Provides the starting point for constructing Gremlin queries, including vertex and edge traversals.
    /// </summary>
    public interface IStartGremlinQuery
    {
        /// <summary>
        /// Adds a new vertex to the graph with the specified vertex instance.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex to add.</typeparam>
        /// <param name="vertex">The vertex instance to add.</param>
        /// <returns>A query that continues with the added vertex.</returns>
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        
        /// <summary>
        /// Adds a new vertex to the graph of the specified type using its parameterless constructor.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex to add.</typeparam>
        /// <returns>A query that continues with the added vertex.</returns>
        IVertexGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new();

        /// <summary>
        /// Adds a new edge to the graph with the specified edge instance.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge to add.</typeparam>
        /// <param name="edge">The edge instance to add.</param>
        /// <returns>A query that continues with the added edge.</returns>
        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        
        /// <summary>
        /// Adds a new edge to the graph of the specified type using its parameterless constructor.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge to add.</typeparam>
        /// <returns>A query that continues with the added edge.</returns>
        IEdgeGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new();

        /// <summary>
        /// Gets administrative access to the query for advanced manipulation.
        /// </summary>
        /// <returns>The administrative interface for this query.</returns>
        IGremlinQueryAdmin AsAdmin();

        /// <summary>
        /// Starts a traversal from the edge with the specified identifier.
        /// </summary>
        /// <param name="id">The edge identifier.</param>
        /// <returns>A query that continues with the edge.</returns>
        IEdgeGremlinQuery<object> E(object id);
        
        /// <summary>
        /// Starts a traversal from the edges with the specified identifiers.
        /// </summary>
        /// <param name="ids">The edge identifiers.</param>
        /// <returns>A query that continues with the edges.</returns>
        IEdgeGremlinQuery<object> E(params object[] ids);
        
        /// <summary>
        /// Starts a traversal from the edges with the specified identifiers.
        /// </summary>
        /// <param name="ids">The edge identifiers.</param>
        /// <returns>A query that continues with the edges.</returns>
        IEdgeGremlinQuery<object> E(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Starts a traversal from the edge with the specified identifier and type.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge.</typeparam>
        /// <param name="id">The edge identifier.</param>
        /// <returns>A query that continues with the edge.</returns>
        IEdgeGremlinQuery<TEdge> E<TEdge>(object id);
        
        /// <summary>
        /// Starts a traversal from the edges with the specified identifiers and type.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edges.</typeparam>
        /// <param name="ids">The edge identifiers.</param>
        /// <returns>A query that continues with the edges.</returns>
        IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        
        /// <summary>
        /// Starts a traversal from the edges with the specified identifiers and type.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edges.</typeparam>
        /// <param name="ids">The edge identifiers.</param>
        /// <returns>A query that continues with the edges.</returns>
        IEdgeGremlinQuery<TEdge> E<TEdge>(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Starts a traversal from the vertex with the specified identifier.
        /// </summary>
        /// <param name="id">The vertex identifier.</param>
        /// <returns>A query that continues with the vertex.</returns>
        IVertexGremlinQuery<object> V(object id);
        
        /// <summary>
        /// Starts a traversal from the vertices with the specified identifiers.
        /// </summary>
        /// <param name="ids">The vertex identifiers.</param>
        /// <returns>A query that continues with the vertices.</returns>
        IVertexGremlinQuery<object> V(params object[] ids);
        
        /// <summary>
        /// Starts a traversal from the vertices with the specified identifiers.
        /// </summary>
        /// <param name="ids">The vertex identifiers.</param>
        /// <returns>A query that continues with the vertices.</returns>
        IVertexGremlinQuery<object> V(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Starts a traversal from the vertex with the specified identifier and type.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex.</typeparam>
        /// <param name="id">The vertex identifier.</param>
        /// <returns>A query that continues with the vertex.</returns>
        IVertexGremlinQuery<TVertex> V<TVertex>(object id);
        
        /// <summary>
        /// Starts a traversal from the vertices with the specified identifiers and type.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices.</typeparam>
        /// <param name="ids">The vertex identifiers.</param>
        /// <returns>A query that continues with the vertices.</returns>
        IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        
        /// <summary>
        /// Starts a traversal from the vertices with the specified identifiers and type.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices.</typeparam>
        /// <param name="ids">The vertex identifiers.</param>
        /// <returns>A query that continues with the vertices.</returns>
        IVertexGremlinQuery<TVertex> V<TVertex>(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Injects the specified elements into the traversal stream.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements to inject.</typeparam>
        /// <param name="elements">The elements to inject.</param>
        /// <returns>A query that continues with the injected elements.</returns>
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
        
        /// <summary>
        /// Injects the specified elements into the traversal stream.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements to inject.</typeparam>
        /// <param name="elements">The elements to inject.</param>
        /// <returns>A query that continues with the injected elements.</returns>
        IGremlinQuery<TElement> Inject<TElement>(params ReadOnlySpan<TElement> elements);

        /// <summary>
        /// Replaces an edge in the graph with the specified edge instance (upsert operation).
        /// </summary>
        /// <typeparam name="TNewEdge">The type of the edge to replace.</typeparam>
        /// <param name="edge">The edge instance to replace.</param>
        /// <returns>A query that continues with the replaced edge.</returns>
        IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);
        
        /// <summary>
        /// Replaces a vertex in the graph with the specified vertex instance (upsert operation).
        /// </summary>
        /// <typeparam name="TNewVertex">The type of the vertex to replace.</typeparam>
        /// <param name="vertex">The vertex instance to replace.</param>
        /// <returns>A query that continues with the replaced vertex.</returns>
        IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(TNewVertex vertex);
    }
}
