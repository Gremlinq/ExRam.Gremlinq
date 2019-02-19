using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IPropertyGremlinQuery<TElement> : IGremlinQuery<TElement>
    {
        IPropertyGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }
}
