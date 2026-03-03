namespace ExRam.Gremlinq.Core
{
    /// <summary>Provides base operations for queries over edges where the incoming (head) vertex is known.</summary>
    public interface IInEdgeGremlinQueryBase :
        IEdgeGremlinQueryBase
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<object> Lower();
    }

    /// <summary>Provides typed base operations for in-edge queries with edge type <typeparamref name="TEdge"/> and incoming vertex type <typeparamref name="TInVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (head) vertex type.</typeparam>
    public interface IInEdgeGremlinQueryBase<TEdge, TInVertex> :
        IInEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <summary>
        /// Specifies the outgoing vertex of the edge via a traversal.
        /// Corresponds to the Gremlin <c>from()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="fromVertexTraversal">The traversal that selects the outgoing vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);

        /// <summary>
        /// Map the edge to its incoming/head incident vertex, typed as <typeparamref name="TInVertex"/>.
        /// Corresponds to the Gremlin <c>inV()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        new IVertexGremlinQuery<TInVertex> InV();

        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();
    }

    /// <summary>Provides recursive (CRTP) base operations for in-edge queries.</summary>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IInEdgeGremlinQueryBaseRec<TSelf> :
        IInEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
            where TSelf : IInEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>Provides recursive (CRTP) typed base operations for in-edge queries.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (head) vertex type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> :
        IInEdgeGremlinQueryBaseRec<TSelf>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
            where TSelf : IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf>;

    /// <summary>A query over edges of type <typeparamref name="TEdge"/> where the incoming vertex is of type <typeparamref name="TInVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (head) vertex type.</typeparam>
    public interface IInEdgeGremlinQuery<TEdge, TInVertex> :
        IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, IInEdgeGremlinQuery<TEdge, TInVertex>>;
}
