using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex> : IEdgeGremlinQuery<TEdge>
    {
        new IVertexGremlinQuery<TOutVertex> OutV();

        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> To<TInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TInVertex>> toVertexTraversal);
    }
}
