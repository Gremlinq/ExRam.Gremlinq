using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IPropertyGremlinQueryBase : IGremlinQueryBase
    {
        new IPropertyGremlinQuery<TResult> Cast<TResult>();
    }

    public interface IPropertyGremlinQueryBase<TElement> :
        IPropertyGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        IValueGremlinQuery<string> Key();

        IValueGremlinQuery<object> Value();
        IValueGremlinQuery<TValue> Value<TValue>();

        IPropertyGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }

    public interface IPropertyGremlinQuery<TElement> :
        IPropertyGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>
    {

    }
}
