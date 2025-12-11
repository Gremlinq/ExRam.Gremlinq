namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for incoming edges (edges pointing to a vertex).
    /// </summary>
    public interface IInEdgeGremlinQueryBase :
        IEdgeGremlinQueryBase
    {
        /// <summary>
        /// Removes incoming edge type information from the query.
        /// </summary>
        /// <returns>A general edge query.</returns>
        new IEdgeGremlinQuery<object> Lower();
    }

    /// <summary>
    /// Represents a strongly-typed query for incoming edges with known incoming vertex type.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (target) vertex type.</typeparam>
    public interface IInEdgeGremlinQueryBase<TEdge, TInVertex> :
        IInEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <summary>
        /// Sets the outgoing (source) vertex of the edge, creating a fully-typed edge query.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="fromVertexTraversal">A traversal starting from the incoming vertex that selects the outgoing vertex.</param>
        /// <returns>An edge query with both vertex types specified.</returns>
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);

        /// <summary>
        /// Navigates to the incoming (target) vertices of the edges.
        /// </summary>
        /// <returns>A query for the typed incoming vertices.</returns>
        new IVertexGremlinQuery<TInVertex> InV();

        /// <summary>
        /// Removes incoming edge type information from the query.
        /// </summary>
        /// <returns>An edge query without vertex type constraints.</returns>
        new IEdgeGremlinQuery<TEdge> Lower();
    }

    /// <summary>
    /// Represents a recursive incoming edge query.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IInEdgeGremlinQueryBaseRec<TSelf> :
        IInEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IInEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>
    /// Represents a recursive strongly-typed incoming edge query.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (target) vertex type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> :
        IInEdgeGremlinQueryBaseRec<TSelf>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf>;

    /// <summary>
    /// Represents a query for strongly-typed incoming edges with known incoming vertex type and full edge operations.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (target) vertex type.</typeparam>
    public interface IInEdgeGremlinQuery<TEdge, TInVertex> :
        IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, IInEdgeGremlinQuery<TEdge, TInVertex>>;
}
