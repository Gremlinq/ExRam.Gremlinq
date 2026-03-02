namespace ExRam.Gremlinq.Core
{
    //TODO: Rename.
    public interface IInOrOutEdgeGremlinQueryBase :
        IEdgeGremlinQueryBase
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<object> Lower();
    }

    public interface IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Specifies the outgoing vertex of the edge via a traversal.
        /// Corresponds to the Gremlin <c>from()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="fromVertexTraversal">The traversal that selects the outgoing vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IVertexGremlinQueryBase<TTargetVertex>> fromVertexTraversal);

        /// <summary>
        /// Specifies the outgoing vertex of the edge via a step label.
        /// Corresponds to the Gremlin <c>from()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the outgoing vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        new IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        /// <summary>
        /// Specifies the incoming vertex of the edge via a traversal.
        /// Corresponds to the Gremlin <c>to()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the incoming vertex.</typeparam>
        /// <param name="toVertexTraversal">The traversal that selects the incoming vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal);

        /// <summary>
        /// Specifies the incoming vertex of the edge via a step label.
        /// Corresponds to the Gremlin <c>to()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TTargetVertex">The type of the incoming vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the incoming vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);
    }

    public interface IInOrOutEdgeGremlinQueryBaseRec<TSelf> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
            where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TSelf>;

    public interface IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> :
        IInOrOutEdgeGremlinQueryBaseRec<TSelf>,
        IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
            where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf>;

    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>;
}
