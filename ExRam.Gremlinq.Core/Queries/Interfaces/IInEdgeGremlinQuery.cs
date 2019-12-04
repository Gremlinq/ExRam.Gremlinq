using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex> : IEdgeGremlinQuery<TEdge>
    {
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IGremlinQuery<TOutVertex>> fromVertexTraversal);

        new IVertexGremlinQuery<TInVertex> InV();
    }
}
