namespace ExRam.Gremlinq.Core
{
    /// <summary>Provides graph traversal source operations for adding vertices and edges, and reading elements by id.</summary>
    public interface IStartGremlinQuery
    {
        /// <summary>
        /// Adds a vertex to the graph with property values from the provided instance.
        /// Corresponds to the Gremlin <c>addV()</c> step.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex to add.</typeparam>
        /// <param name="vertex">The vertex instance containing the property values to set.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addvertex-step">Reference Documentation - AddVertex Step</seealso>
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);

        /// <summary>
        /// Adds a vertex to the graph.
        /// Corresponds to the Gremlin <c>addV()</c> step.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertex to add. Must have a parameterless constructor.</typeparam>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addvertex-step">Reference Documentation - AddVertex Step</seealso>
        IVertexGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new();

        /// <summary>
        /// Adds an edge to the graph with property values from the provided instance.
        /// Corresponds to the Gremlin <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge to add.</typeparam>
        /// <param name="edge">The edge instance containing the property values to set.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);

        /// <summary>
        /// Adds an edge to the graph.
        /// Corresponds to the Gremlin <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TEdge">The type of the edge to add. Must have a parameterless constructor.</typeparam>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IEdgeGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new();

        /// <summary>
        /// Get access to administrative methods of the query.
        /// </summary>
        IGremlinQueryAdmin AsAdmin();

        /// <summary>
        /// Reads an edge from the graph by its identifier.
        /// Corresponds to the Gremlin <c>E()</c> step.
        /// </summary>
        /// <param name="id">The identifier of the edge to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IEdgeGremlinQuery<object> E(object id);

        /// <summary>
        /// Reads edges from the graph by their identifiers.
        /// Corresponds to the Gremlin <c>E()</c> step.
        /// </summary>
        /// <param name="ids">The identifiers of the edges to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IEdgeGremlinQuery<object> E(params object[] ids);

        /// <inheritdoc cref="E(object[])" />
        IEdgeGremlinQuery<object> E(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Reads an edge from the graph by its identifier, typed as <typeparamref name="TEdge"/>.
        /// Corresponds to the Gremlin <c>E()</c> step.
        /// </summary>
        /// <typeparam name="TEdge">The expected type of the edge.</typeparam>
        /// <param name="id">The identifier of the edge to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IEdgeGremlinQuery<TEdge> E<TEdge>(object id);

        /// <summary>
        /// Reads edges from the graph by their identifiers, typed as <typeparamref name="TEdge"/>.
        /// Corresponds to the Gremlin <c>E()</c> step.
        /// </summary>
        /// <typeparam name="TEdge">The expected type of the edges.</typeparam>
        /// <param name="ids">The identifiers of the edges to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);

        /// <inheritdoc cref="E{TEdge}(object[])" />
        IEdgeGremlinQuery<TEdge> E<TEdge>(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Reads a vertex from the graph by its identifier.
        /// Corresponds to the Gremlin <c>V()</c> step.
        /// </summary>
        /// <param name="id">The identifier of the vertex to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IVertexGremlinQuery<object> V(object id);

        /// <summary>
        /// Reads vertices from the graph by their identifiers.
        /// Corresponds to the Gremlin <c>V()</c> step.
        /// </summary>
        /// <param name="ids">The identifiers of the vertices to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IVertexGremlinQuery<object> V(params object[] ids);

        /// <inheritdoc cref="V(object[])" />
        IVertexGremlinQuery<object> V(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Reads a vertex from the graph by its identifier, typed as <typeparamref name="TVertex"/>.
        /// Corresponds to the Gremlin <c>V()</c> step.
        /// </summary>
        /// <typeparam name="TVertex">The expected type of the vertex.</typeparam>
        /// <param name="id">The identifier of the vertex to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IVertexGremlinQuery<TVertex> V<TVertex>(object id);

        /// <summary>
        /// Reads vertices from the graph by their identifiers, typed as <typeparamref name="TVertex"/>.
        /// Corresponds to the Gremlin <c>V()</c> step.
        /// </summary>
        /// <typeparam name="TVertex">The expected type of the vertices.</typeparam>
        /// <param name="ids">The identifiers of the vertices to retrieve.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#graph-step">Reference Documentation - Graph Step</seealso>
        IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);

        /// <inheritdoc cref="V{TVertex}(object[])" />
        IVertexGremlinQuery<TVertex> V<TVertex>(params ReadOnlySpan<object> ids);

        /// <summary>
        /// Provides a way to add arbitrary objects to a traversal stream.
        /// Corresponds to the Gremlin <c>inject()</c> step.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements to inject.</typeparam>
        /// <param name="elements">The objects to add to the stream.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#inject-step">Reference Documentation - Inject Step</seealso>
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);

        /// <inheritdoc cref="Inject{TElement}(TElement[])" />
        IGremlinQuery<TElement> Inject<TElement>(params ReadOnlySpan<TElement> elements);

        /// <summary>
        /// Replaces an existing edge in the graph with updated property values.
        /// </summary>
        /// <typeparam name="TNewEdge">The type of the edge.</typeparam>
        /// <param name="edge">The edge instance containing the updated property values.</param>
        IEdgeGremlinQuery<TNewEdge> ReplaceE<TNewEdge>(TNewEdge edge);

        /// <summary>
        /// Replaces an existing vertex in the graph with updated property values.
        /// </summary>
        /// <typeparam name="TNewVertex">The type of the vertex.</typeparam>
        /// <param name="vertex">The vertex instance containing the updated property values.</param>
        IVertexGremlinQuery<TNewVertex> ReplaceV<TNewVertex>(TNewVertex vertex);
    }
}
