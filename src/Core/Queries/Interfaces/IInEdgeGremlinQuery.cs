namespace ExRam.Gremlinq.Core
{
    public interface IInEdgeGremlinQueryBase :
        IEdgeGremlinQueryBase
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<object> Lower();
    }

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

    public interface IInEdgeGremlinQueryBaseRec<TSelf> :
        IInEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
            where TSelf : IInEdgeGremlinQueryBaseRec<TSelf>;

    public interface IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf> :
        IInEdgeGremlinQueryBaseRec<TSelf>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
            where TSelf : IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, TSelf>;

    public interface IInEdgeGremlinQuery<TEdge, TInVertex> :
        IInEdgeGremlinQueryBaseRec<TEdge, TInVertex, IInEdgeGremlinQuery<TEdge, TInVertex>>;
}
