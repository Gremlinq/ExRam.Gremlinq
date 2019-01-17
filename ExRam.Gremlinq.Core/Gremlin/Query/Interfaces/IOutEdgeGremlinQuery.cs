using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex> : IEdgeGremlinQuery<TEdge>
    {
        new IVertexGremlinQuery<TOutVertex> OutV();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
    }
}
