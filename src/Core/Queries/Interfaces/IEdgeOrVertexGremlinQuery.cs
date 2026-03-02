namespace ExRam.Gremlinq.Core
{
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

    public interface IEdgeOrVertexGremlinQueryBaseRec<TSelf> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>;

    public interface IEdgeOrVertexGremlinQueryBase<TElement> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBase<TElement>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IElementGremlinQuery<TElement> Lower();
    }

    public interface IEdgeOrVertexGremlinQueryBaseRec<TElement, TSelf> :
        IEdgeOrVertexGremlinQueryBaseRec<TSelf>,
        IEdgeOrVertexGremlinQueryBase<TElement>,
        IElementGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>;

    public interface IEdgeOrVertexGremlinQuery<TElement> :
        IEdgeOrVertexGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>;
}
