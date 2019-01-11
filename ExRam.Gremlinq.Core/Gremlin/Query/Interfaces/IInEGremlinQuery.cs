using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        new IEGremlinQuery<TEdge, TOutVertex, TAdjacentVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);

        new IVGremlinQuery<TAdjacentVertex> InV();
    }
}
