namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for outgoing edges (edges originating from a vertex).
    /// </summary>
    public interface IOutEdgeGremlinQueryBase
        : IEdgeGremlinQueryBase
    {
        /// <summary>
        /// Removes outgoing edge type information from the query.
        /// </summary>
        /// <returns>A general edge query.</returns>
        new IEdgeGremlinQuery<object> Lower();
    }

    /// <summary>
    /// Represents a strongly-typed query for outgoing edges with known outgoing vertex type.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (source) vertex type.</typeparam>
    public interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <summary>
        /// Removes outgoing edge type information from the query.
        /// </summary>
        /// <returns>An edge query without vertex type constraints.</returns>
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Navigates to the outgoing (source) vertices of the edges.
        /// </summary>
        /// <returns>A query for the typed outgoing vertices.</returns>
        new IVertexGremlinQuery<TOutVertex> OutV();

        /// <summary>
        /// Sets the incoming (target) vertex of the edge, creating a fully-typed edge query.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <param name="toVertexTraversal">A traversal starting from the outgoing vertex that selects the incoming vertex.</param>
        /// <returns>An edge query with both vertex types specified.</returns>
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
    }

    /// <summary>
    /// Represents a recursive outgoing edge query.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IOutEdgeGremlinQueryBaseRec<TSelf> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IOutEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>
    /// Represents a recursive strongly-typed outgoing edge query.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (source) vertex type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> :
        IOutEdgeGremlinQueryBaseRec<TSelf>,
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf>;

    /// <summary>
    /// Represents a query for strongly-typed outgoing edges with known outgoing vertex type and full edge operations.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (source) vertex type.</typeparam>
    public interface IOutEdgeGremlinQuery<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, IOutEdgeGremlinQuery<TEdge, TOutVertex>>;
}
