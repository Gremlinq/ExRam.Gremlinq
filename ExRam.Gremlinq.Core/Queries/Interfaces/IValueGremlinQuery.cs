using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IValueGremlinQueryBase :
        IGremlinQueryBase
    {
        new IValueGremlinQuery<TResult> Cast<TResult>();
    }

    public interface IValueGremlinQueryBase<TElement> :
        IValueGremlinQueryBase,
        IGremlinQueryBase<TElement>
    {
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQueryBase;

        IValueGremlinQuery<object> SumLocal();
        IValueGremlinQuery<TElement> Sum();

        IValueGremlinQuery<object> MinLocal();
        IValueGremlinQuery<TElement> Min();

        IValueGremlinQuery<object> MaxLocal();
        IValueGremlinQuery<TElement> Max();

        IValueGremlinQuery<object> MeanLocal();
        IValueGremlinQuery<TElement> Mean();

        IValueGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }

    public interface IValueGremlinQueryBaseRec<TSelf> :
        IValueGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IValueGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IValueGremlinQueryBaseRec<TElement, TSelf> :
        IValueGremlinQueryBaseRec<TSelf>,
        IValueGremlinQueryBase<TElement>,
        IGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IValueGremlinQueryBaseRec<TElement, TSelf>
    {
        TSelf Order(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);
        TSelf OrderLocal(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);
    }

    public interface IValueGremlinQuery<TElement> :
        IValueGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>
    {

    }
}
