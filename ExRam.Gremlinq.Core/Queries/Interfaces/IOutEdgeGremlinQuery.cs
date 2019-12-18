using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOutEdgeGremlinQueryBase
        : IEdgeGremlinQueryBase
    {

    }

    public partial interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        new IVertexGremlinQuery<TOutVertex> OutV();

        IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TInVertex>> toVertexTraversal);
    }

    public partial interface IOutEdgeGremlinQueryBaseRec<TSelf> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IOutEdgeGremlinQueryBaseRec<TSelf>
    {

    }

    public partial interface IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> :
        IOutEdgeGremlinQueryBaseRec<TSelf>,
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf>
    {

    }

    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, IOutEdgeGremlinQuery<TEdge, TOutVertex>>
    {

    }
}
