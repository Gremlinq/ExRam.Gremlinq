using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> : IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }
}