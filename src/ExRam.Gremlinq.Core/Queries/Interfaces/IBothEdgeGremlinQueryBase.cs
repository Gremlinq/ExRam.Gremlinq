namespace ExRam.Gremlinq.Core
{
    public interface IBothEdgeGremlinQueryBase :
        IInOrOutEdgeGremlinQueryBase,
        IOutEdgeGremlinQueryBase,
        IInEdgeGremlinQueryBase
    {
        new IEdgeOrVertexGremlinQuery<object> Lower();
    }
    
    public interface IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> :
        IBothEdgeGremlinQueryBase,
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>
    {
        new IEdgeOrVertexGremlinQuery<TEdge> Lower();
    }
    
    public interface IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IOutEdgeGremlinQueryBaseRec<IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IInEdgeGremlinQueryBaseRec<IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>
    {

    }
}
