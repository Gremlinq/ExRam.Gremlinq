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

    }

    public interface IValueGremlinQuery<TElement> :
        IValueGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>
    {

    }
}
