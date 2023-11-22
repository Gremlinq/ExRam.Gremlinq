namespace ExRam.Gremlinq.Core
{
    public interface IArrayGremlinQueryBase : IGremlinQueryBase
    {
        IValueGremlinQuery<object> Unfold();

        new IValueGremlinQuery<object[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TSelf> : IArrayGremlinQueryBase, IGremlinQueryBaseRec<TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TSelf>
    {
        TSelf LimitLocal(long count);

        TSelf RangeLocal(long low, long high);

        TSelf SkipLocal(long count);

        TSelf TailLocal(long count);
    }

    public interface IArrayGremlinQueryBase<TArrayItem> : IArrayGremlinQueryBase
    {
        new IValueGremlinQuery<TArrayItem> Unfold();

        new IValueGremlinQuery<TArrayItem[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArrayItem, TSelf> :
        IArrayGremlinQueryBase<TArrayItem>,
        IArrayGremlinQueryBaseRec<TSelf> where TSelf : IArrayGremlinQueryBaseRec<TArrayItem, TSelf>
    {

    }

    public interface IArrayGremlinQueryBase<TArray, TArrayItem> :
        IArrayGremlinQueryBase<TArrayItem>,
        IGremlinQueryBase<TArray>
    {
        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem>,
        IArrayGremlinQueryBaseRec<TArrayItem, TSelf>,
        IGremlinQueryBaseRec<TArray, TSelf> where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>
    {
    }

    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> :
        IArrayGremlinQueryBase<TArray, TArrayItem>
    {
        TOriginalQuery SumLocal();

        TOriginalQuery MinLocal();

        TOriginalQuery MaxLocal();

        TOriginalQuery MeanLocal();

        new TOriginalQuery Unfold();

        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TOriginalQuery, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>,
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>  where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem,  TOriginalQuery, TSelf>
    {
     
    }

    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> :
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>
    {

    }
}
