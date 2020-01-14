namespace ExRam.Gremlinq.Core
{
    public interface IArrayGremlinQueryBase : IGremlinQueryBase
    {
        new IGremlinQuery<object[]> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TSelf> :
        IArrayGremlinQueryBase,
        IGremlinQueryBaseRec<TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TSelf>
    {

    }

    public partial interface IArrayGremlinQueryBase<TArray, TQuery> :
        IArrayGremlinQueryBase,
        IGremlinQueryBase<TArray>
    {
        TQuery Unfold();
        new IGremlinQuery<TArray> Lower();
    }

    public interface IArrayGremlinQueryBaseRec<TArray, TQuery, TSelf> :
        IArrayGremlinQueryBaseRec<TSelf>,
        IArrayGremlinQueryBase<TArray, TQuery>,
        IGremlinQueryBaseRec<TArray, TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TArray, TQuery, TSelf>
    {

    }

    public interface IArrayGremlinQuery<TArray, TQuery> :
        IArrayGremlinQueryBaseRec<TArray, TQuery, IArrayGremlinQuery<TArray, TQuery>>
    {

    }
}
