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

    public interface IArrayGremlinQueryBase<TArray, TArrayItem> :
        IArrayGremlinQueryBase<TArrayItem>,
        IValueGremlinQueryBase<TArray>
    {
        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> :
        IArrayGremlinQueryBase<TArray, TArrayItem>
    {
        new TOriginalQuery SumLocal();

        new TOriginalQuery MinLocal();

        new TOriginalQuery MaxLocal();

        new TOriginalQuery MeanLocal();

        new TOriginalQuery Unfold();

        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TOriginalQuery, TSelf> :
        IArrayGremlinQueryBaseRec<TSelf>,
        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>,
        IValueGremlinQueryBaseRec<TArray, TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, TSelf>
    {

    }

    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> :
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>
    {

    }
}
