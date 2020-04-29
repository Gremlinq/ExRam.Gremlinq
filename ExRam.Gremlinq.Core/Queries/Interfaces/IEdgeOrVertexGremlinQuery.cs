namespace ExRam.Gremlinq.Core
{
    public partial interface IEdgeOrVertexGremlinQueryBase :
        IElementGremlinQueryBase
    {
        new IEdgeOrVertexGremlinQuery<TResult> Cast<TResult>();

        new IElementGremlinQuery<object> Lower();
    }

    public interface IEdgeOrVertexGremlinQueryBaseRec<TSelf> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>
    {
    }

    public partial interface IEdgeOrVertexGremlinQueryBase<TElement> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBase<TElement>
    {
        new IElementGremlinQuery<TElement> Lower();
    }

    public interface IEdgeOrVertexGremlinQueryBaseRec<TElement, TSelf> :
        IEdgeOrVertexGremlinQueryBaseRec<TSelf>,
        IEdgeOrVertexGremlinQueryBase<TElement>,
        IElementGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>
    {
    }

    public interface IEdgeOrVertexGremlinQuery<TElement> :
        IEdgeOrVertexGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>
    {
    }
}
