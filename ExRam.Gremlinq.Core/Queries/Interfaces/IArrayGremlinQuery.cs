namespace ExRam.Gremlinq.Core
{
    public interface IArrayGremlinQueryBase : IValueGremlinQueryBase
    {
        new IValueGremlinQuery<object[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TSelf> :
        IArrayGremlinQueryBase,
        IValueGremlinQueryBaseRec<TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TSelf>
    {

    }

    public partial interface IArrayGremlinQueryBase<TArray, out TQuery> :
        IArrayGremlinQueryBase,
        IValueGremlinQueryBase<TArray>
    {
        TQuery Unfold();
        new IValueGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, out TQuery, TSelf> :
        IArrayGremlinQueryBaseRec<TSelf>,
        IArrayGremlinQueryBase<TArray, TQuery>,
        IValueGremlinQueryBaseRec<TArray, TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TArray, TQuery, TSelf>
    {

    }

    public interface IArrayGremlinQuery<TArray, TQuery> :
        IArrayGremlinQueryBaseRec<TArray, TQuery, IArrayGremlinQuery<TArray, TQuery>>
    {

    }
}
