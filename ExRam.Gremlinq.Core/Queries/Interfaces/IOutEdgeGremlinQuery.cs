using System;

namespace ExRam.Gremlinq.Core
{
    public interface IOutEdgeGremlinQueryBase
        : IEdgeGremlinQueryBase
    {
        new IEdgeGremlinQuery<object> Lower();
    }

    public partial interface IOutEdgeGremlinQueryBase<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Lower();

        new IVertexGremlinQuery<TOutVertex> OutV();

        IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TInVertex>> toVertexTraversal);
    }

    public interface IOutEdgeGremlinQueryBaseRec<TSelf> :
        IOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IOutEdgeGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf> :
        IOutEdgeGremlinQueryBaseRec<TSelf>,
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, TSelf>
    {

    }

    public interface IOutEdgeGremlinQuery<TEdge, TOutVertex> :
        IOutEdgeGremlinQueryBaseRec<TEdge, TOutVertex, IOutEdgeGremlinQuery<TEdge, TOutVertex>>
    {

    }
}
