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
    }

    public interface IArrayGremlinQueryBase<TArrayItem> : IArrayGremlinQueryBase
    {
        new IValueGremlinQuery<TArrayItem> Unfold();

        new IValueGremlinQuery<TArrayItem[]> Lower();
    }

    public interface IArrayGremlinQueryBase<TArray, TArrayItem> :
        IArrayGremlinQueryBase<TArrayItem>,
        IGremlinQueryBase<TArray>
    {
        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBase<TArray, TArrayItem, out TOriginalQuery> :
        IArrayGremlinQueryBase<TArray, TArrayItem>
    {
        TOriginalQuery SumLocal();

        TOriginalQuery MinLocal();

        TOriginalQuery MaxLocal();

        TOriginalQuery MeanLocal();

        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery> LimitLocal(long count);

        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery> RangeLocal(long low, long high);

        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery> SkipLocal(long count);

        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery> TailLocal(long count);

        new TOriginalQuery Unfold();

        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery> :
        IArrayGremlinQueryBaseRec<IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>,
        IArrayGremlinQueryBase<TArray, TArrayItem, TOriginalQuery>,
        IGremlinQueryBaseRec<TArray, IArrayGremlinQuery<TArray, TArrayItem, TOriginalQuery>>
    {

    }
}
