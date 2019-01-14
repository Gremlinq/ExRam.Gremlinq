using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IInEdgeGremlinQuery<TEdge, TAdjacentVertex> : IEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TEdge, TOutVertex, TAdjacentVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);

        new IVertexGremlinQuery<TAdjacentVertex> InV();
    }
}
