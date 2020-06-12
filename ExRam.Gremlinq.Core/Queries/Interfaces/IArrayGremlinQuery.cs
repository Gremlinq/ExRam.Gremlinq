namespace ExRam.Gremlinq.Core
{
    public interface IArrayGremlinQueryBase : IValueGremlinQueryBase
    {
        IValueGremlinQuery<object> Unfold();
        new IValueGremlinQuery<object[]> Lower();
    }

    public interface IArrayGremlinQueryBase<TArrayItem> : IArrayGremlinQueryBase
    {
        new IValueGremlinQuery<TArrayItem> Unfold();
        new IValueGremlinQuery<TArrayItem[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TSelf> :
        IArrayGremlinQueryBase,
        IValueGremlinQueryBaseRec<TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TQuery> :
        IArrayGremlinQueryBase<TArrayItem>,
        IValueGremlinQueryBase<TArray>
    {
        new TQuery SumLocal();

        new TQuery MinLocal();

        new TQuery MaxLocal();

        new TQuery MeanLocal();

        new TQuery Unfold();

        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TQuery, TSelf> :
        IArrayGremlinQueryBaseRec<TSelf>,
        IArrayGremlinQueryBase<TArray, TArrayItem, TQuery>,
        IValueGremlinQueryBaseRec<TArray, TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem, TQuery, TSelf>
    {

    }

    public interface IArrayGremlinQuery<TArray, TArrayItem, TQuery> :
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TQuery, IArrayGremlinQuery<TArray, TArrayItem, TQuery>>
    {

    }
}
