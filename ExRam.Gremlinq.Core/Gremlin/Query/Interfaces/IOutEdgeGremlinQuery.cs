using System;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOutEdgeGremlinQuery<TEdge, TAdjacentVertex> : IEdgeGremlinQuery<TEdge>
    {
        new IVertexGremlinQuery<TAdjacentVertex> OutV();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
    }
}
