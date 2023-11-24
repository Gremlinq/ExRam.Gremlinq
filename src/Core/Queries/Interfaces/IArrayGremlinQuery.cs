namespace ExRam.Gremlinq.Core
{
    public interface IArrayGremlinQueryBase : IGremlinQueryBase
    {
        IGremlinQuery<object> Unfold();

        IGremlinQuery<object> SumLocal();

        IGremlinQuery<object> MinLocal();

        IGremlinQuery<object> MaxLocal();

        IGremlinQuery<object> MeanLocal();

        new IGremlinQuery<object[]> Lower();
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
        new IGremlinQuery<TArrayItem> Unfold();

        new IGremlinQuery<TArrayItem> SumLocal();

        new IGremlinQuery<TArrayItem> MinLocal();

        new IGremlinQuery<TArrayItem> MaxLocal();

        new IGremlinQuery<TArrayItem> MeanLocal();

        new IGremlinQuery<TArrayItem[]> Lower();
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
        new IGremlinQuery<TArray> Lower();
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
        new TOriginalQuery SumLocal();

        new TOriginalQuery MinLocal();

        new TOriginalQuery MaxLocal();

        new TOriginalQuery MeanLocal();

        new TOriginalQuery Unfold();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TArrayItem, out TOriginalQuery, TSelf> :
        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>,
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TSelf>
            where TOriginalQuery : IGremlinQueryBase
            where TSelf : IArrayGremlinQueryBaseRec<TArray, TArrayItem,  TOriginalQuery, TSelf>
    {
     
    }

    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> :
        IArrayGremlinQueryBaseRec<TArray, TArrayItem, TOriginalQuery, IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>
            where TOriginalQuery : IGremlinQueryBase
    {

    }
}
