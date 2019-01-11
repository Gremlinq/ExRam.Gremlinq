using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderedEGremlinQuery<TEdge> : IEGremlinQuery<TEdge>
    {
        IOrderedEGremlinQuery<TEdge> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge> ThenBy(string lambda);
        IOrderedEGremlinQuery<TEdge> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }

    public interface IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(string lambda);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }
}
