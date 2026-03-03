namespace ExRam.Gremlinq.Core
{
    /// <summary>Provides base operations for queries over edges or vertices.</summary>
    public interface IEdgeOrVertexGremlinQueryBase :
        IElementGremlinQueryBase
    {
        /// <inheritdoc cref="IGremlinQueryBase.Cast{TResult}" />
        new IEdgeOrVertexGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Returns the query typed as a lower (less specific) query type.
        /// </summary>
        new IElementGremlinQuery<object> Lower();
    }

    /// <summary>Provides recursive (CRTP) base operations for edge-or-vertex queries.</summary>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IEdgeOrVertexGremlinQueryBaseRec<TSelf> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>;

    /// <summary>Provides typed base operations for edge-or-vertex queries carrying elements of type <typeparamref name="TElement"/>.</summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public interface IEdgeOrVertexGremlinQueryBase<TElement> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBase<TElement>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IElementGremlinQuery<TElement> Lower();
    }

    /// <summary>Provides recursive (CRTP) typed base operations for edge-or-vertex queries.</summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IEdgeOrVertexGremlinQueryBaseRec<TElement, TSelf> :
        IEdgeOrVertexGremlinQueryBaseRec<TSelf>,
        IEdgeOrVertexGremlinQueryBase<TElement>,
        IElementGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>;

    /// <summary>A query over edges or vertices of type <typeparamref name="TElement"/>.</summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public interface IEdgeOrVertexGremlinQuery<TElement> :
        IEdgeOrVertexGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>;
}
