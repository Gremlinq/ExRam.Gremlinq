using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IElementGremlinQueryBase :
        IGremlinQueryBase
    {
        new IElementGremlinQuery<TResult> Cast<TResult>();

        IValueGremlinQuery<object> Id();

        IValueGremlinQuery<string> Label();

        IValueGremlinQuery<TTarget> Values<TTarget>();
        IValueGremlinQuery<object> Values();

        IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>();
        IValueGremlinQuery<IDictionary<string, object>> ValueMap();
    }

    public interface IElementGremlinQueryBaseRec<TSelf> :
        IElementGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>
    {
        TSelf Property(string key, object value);
    }

    public partial interface IElementGremlinQueryBase<TElement> :
        IElementGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        IElementGremlinQuery<TElement> Update(TElement element);

        IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections);
    }

    public interface IElementGremlinQueryBaseRec<TElement, TSelf> :
        IElementGremlinQueryBaseRec<TSelf>,
        IElementGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Order(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);
        TSelf OrderLocal(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);

        TSelf Where(Expression<Func<TElement, bool>> predicate);
        TSelf Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal);

        TSelf Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, TProjectedValue value);
    }

    public interface IElementGremlinQuery<TElement> :
        IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>
    {
    }
}
