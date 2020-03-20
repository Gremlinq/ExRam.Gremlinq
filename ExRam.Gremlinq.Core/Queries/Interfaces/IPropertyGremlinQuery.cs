using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IPropertyGremlinQueryBase : IGremlinQueryBase
    {
        new IPropertyGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IPropertyGremlinQueryBase<TElement> :
        IPropertyGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        IValueGremlinQuery<string> Key();

        IValueGremlinQuery<object> Value();
        IValueGremlinQuery<TValue> Value<TValue>();

        IPropertyGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }

    public interface IPropertyGremlinQueryBaseRec<TSelf> :
        IPropertyGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IPropertyGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IPropertyGremlinQueryBaseRec<TElement, TSelf> :
        IPropertyGremlinQueryBaseRec<TSelf>,
        IPropertyGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IPropertyGremlinQueryBaseRec<TElement, TSelf>
    {

    }

    public interface IPropertyGremlinQuery<TElement> :
        IPropertyGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>
    {

    }
}
