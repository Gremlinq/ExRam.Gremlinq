using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> : IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }
}