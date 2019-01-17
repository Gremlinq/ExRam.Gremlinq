using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex> : IEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);

        new IVertexGremlinQuery<TInVertex> InV();
    }
}
