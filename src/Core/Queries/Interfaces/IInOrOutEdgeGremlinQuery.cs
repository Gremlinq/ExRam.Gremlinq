namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for edges that can be either incoming or outgoing relative to a vertex.
    /// </summary>
    public interface IInOrOutEdgeGremlinQueryBase : IEdgeGremlinQueryBase
    {
        /// <summary>
        /// Removes directional edge type information from the query.
        /// </summary>
        /// <returns>A general edge query.</returns>
        new IEdgeGremlinQuery<object> Lower();
    }

    /// <summary>
    /// Represents a strongly-typed query for edges with a known adjacent vertex type.
    /// The adjacent vertex can be either the source or target of the edge depending on traversal direction.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The type of the adjacent vertex.</typeparam>
    public interface IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <summary>
        /// Removes directional edge type information from the query.
        /// </summary>
        /// <returns>An edge query without vertex type constraints.</returns>
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Sets the source vertex of the edge when the adjacent vertex is the target.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the source vertex.</typeparam>
        /// <param name="fromVertexTraversal">A traversal starting from the adjacent vertex that selects the source vertex.</param>
        /// <returns>An edge query with both vertex types specified.</returns>
        IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IVertexGremlinQueryBase<TTargetVertex>> fromVertexTraversal);
        
        /// <summary>
        /// Sets the source vertex of the edge using a step label when the adjacent vertex is the target.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the source vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the source vertex.</param>
        /// <returns>An edge query with both vertex types specified.</returns>
        new IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        /// <summary>
        /// Sets the target vertex of the edge when the adjacent vertex is the source.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the target vertex.</typeparam>
        /// <param name="toVertexTraversal">A traversal starting from the adjacent vertex that selects the target vertex.</param>
        /// <returns>An edge query with both vertex types specified.</returns>
        IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal);
        
        /// <summary>
        /// Sets the target vertex of the edge using a step label when the adjacent vertex is the source.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the target vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the target vertex.</param>
        /// <returns>An edge query with both vertex types specified.</returns>
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);
    }

    /// <summary>
    /// Represents a recursive in-or-out edge query.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IInOrOutEdgeGremlinQueryBaseRec<TSelf> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>
    /// Represents a recursive strongly-typed in-or-out edge query.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The type of the adjacent vertex.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> :
        IInOrOutEdgeGremlinQueryBaseRec<TSelf>,
        IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf>;

    /// <summary>
    /// Represents a query for strongly-typed edges with a known adjacent vertex type and full edge operations.
    /// </summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The type of the adjacent vertex.</typeparam>
    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>;
}
