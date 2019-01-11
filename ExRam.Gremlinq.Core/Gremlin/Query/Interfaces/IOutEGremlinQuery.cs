using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        new IVGremlinQuery<TAdjacentVertex> OutV();

        new IEGremlinQuery<TEdge, TAdjacentVertex, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
    }
}
