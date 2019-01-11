using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderedVGremlinQuery<TVertex> : IVGremlinQuery<TVertex>
    {
        IOrderedVGremlinQuery<TVertex> ThenBy(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> ThenBy(Func<IGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IOrderedVGremlinQuery<TVertex> ThenBy(string lambda);
        IOrderedVGremlinQuery<TVertex> ThenByDescending(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> ThenByDescending(Func<IGremlinQuery<TVertex>, IGremlinQuery> traversal);
    }
}