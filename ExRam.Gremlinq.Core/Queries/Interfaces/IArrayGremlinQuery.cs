namespace ExRam.Gremlinq.Core
{
    public partial interface IArrayGremlinQueryBase : IGremlinQueryBase
    {
        new IGremlinQuery<object[]> Lower();
    }

    public partial interface IArrayGremlinQueryBaseRec<TSelf> :
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
    }

    public partial interface IArrayGremlinQueryBaseRec<TArray, TQuery, TSelf> :
        IArrayGremlinQueryBaseRec<TSelf>,
        IArrayGremlinQueryBase<TArray, TQuery>,
        IGremlinQueryBaseRec<TArray, TSelf>
        where TSelf : IArrayGremlinQueryBaseRec<TArray, TQuery, TSelf>
    {

    }

    public partial interface IArrayGremlinQuery<TArray, TQuery> :
        IArrayGremlinQueryBaseRec<TArray, TQuery, IArrayGremlinQuery<TArray, TQuery>>
    {

    }
}
