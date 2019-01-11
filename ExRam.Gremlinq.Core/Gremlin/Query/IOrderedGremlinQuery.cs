using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderedGremlinQuery<TElement> : IGremlinQuery<TElement>
    {
        IOrderedGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedGremlinQuery<TElement> ThenBy(string lambda);
        IOrderedGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
    }
}