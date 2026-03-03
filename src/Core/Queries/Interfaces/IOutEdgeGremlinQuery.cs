namespace ExRam.Gremlinq.Core
{
    /// <summary>Provides base operations for queries over edges where the outgoing (tail) vertex is known.</summary>
    public interface IOutEdgeGremlinQueryBase :
        IEdgeGremlinQueryBase
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<object> Lower();
    }

    /// <summary>Provides typed base operations for out-edge queries with edge type <typeparamref name="TEdge"/> and outgoing vertex type <typeparamref name="TOutVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (tail) vertex type.</typeparam>
    public interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Map the edge to its outgoing/tail incident vertex, typed as <typeparamref name="TOutVertex"/>.
        /// Corresponds to the Gremlin <c>outV()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        new IVertexGremlinQuery<TOutVertex> OutV();

        /// <summary>
        /// Specifies the incoming vertex of the edge via a traversal.
        /// Corresponds to the Gremlin <c>to()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <param name="toVertexTraversal">The traversal that selects the incoming vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
    }

    /// <summary>Provides recursive (CRTP) base operations for out-edge queries.</summary>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IOutEdgeGremlinQueryBaseRec<TSelf> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
            where TSelf : IOutEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>Provides recursive (CRTP) typed base operations for out-edge queries.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (tail) vertex type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> :
        IOutEdgeGremlinQueryBaseRec<TSelf>,
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
            where TSelf : IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf>;

    /// <summary>A query over edges of type <typeparamref name="TEdge"/> where the outgoing vertex is of type <typeparamref name="TOutVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (tail) vertex type.</typeparam>
    public interface IOutEdgeGremlinQuery<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, IOutEdgeGremlinQuery<TEdge, TOutVertex>>;
}
