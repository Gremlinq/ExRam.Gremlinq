using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IPropertyGremlinQuery<TElement> : IGremlinQuery<TElement>
    {
        IValueGremlinQuery<string> Key();

        IValueGremlinQuery<object> Value();
        IValueGremlinQuery<TValue> Value<TValue>();

        IPropertyGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }
}
