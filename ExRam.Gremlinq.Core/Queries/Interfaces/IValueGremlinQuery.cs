using System;

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
        IValueGremlinQuery<object> SumLocal();
        IValueGremlinQuery<TElement> Sum();

        IValueGremlinQuery<object> MinLocal();
        IValueGremlinQuery<TElement> Min();

        IValueGremlinQuery<object> MaxLocal();
        IValueGremlinQuery<TElement> Max();

        IValueGremlinQuery<object> MeanLocal();
        IValueGremlinQuery<TElement> Mean();
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
