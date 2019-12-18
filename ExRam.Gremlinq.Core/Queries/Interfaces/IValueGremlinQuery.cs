using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IValueGremlinQueryBase :
        IGremlinQueryBase
    {
    }

    public partial interface IValueGremlinQueryBase<TElement> :
        IValueGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQueryBase;

        IValueGremlinQuery<TElement> SumLocal();
        IValueGremlinQuery<TElement> SumGlobal();

        IValueGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }

    public partial interface IValueGremlinQueryBaseRec<TSelf> :
        IValueGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IValueGremlinQueryBaseRec<TSelf>
    {

    }

    public partial interface IValueGremlinQueryBaseRec<TElement, TSelf> :
        IValueGremlinQueryBaseRec<TSelf>,
        IValueGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IValueGremlinQueryBaseRec<TElement, TSelf>
    {

    }

    public partial interface IValueGremlinQuery<TElement> :
        IValueGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>
    {

    }
}
