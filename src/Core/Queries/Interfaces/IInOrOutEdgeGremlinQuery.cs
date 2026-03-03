namespace ExRam.Gremlinq.Core
{
    //TODO: Rename.
    /// <summary>Provides base operations for edge queries where either the incoming or outgoing vertex is known.</summary>
    public interface IInOrOutEdgeGremlinQueryBase :
        IEdgeGremlinQueryBase
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<object> Lower();
    }

    /// <summary>Provides typed base operations for edge queries with a known adjacent vertex type.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The adjacent vertex type.</typeparam>
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

    /// <summary>Provides recursive (CRTP) base operations for in-or-out edge queries.</summary>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IInOrOutEdgeGremlinQueryBaseRec<TSelf> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
            where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>Provides recursive (CRTP) typed base operations for in-or-out edge queries.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The adjacent vertex type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> :
        IInOrOutEdgeGremlinQueryBaseRec<TSelf>,
        IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
            where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf>;

    /// <summary>A query over edges of type <typeparamref name="TEdge"/> with a known adjacent vertex of type <typeparamref name="TAdjacentVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The adjacent vertex type.</typeparam>
    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>;
}
