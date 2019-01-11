namespace ExRam.Gremlinq.Core
{
    public partial interface IElementGremlinQuery : IGremlinQuery
    {
        IGremlinQuery<object> Id();
    }

    public partial interface IElementGremlinQuery<TElement> : IElementGremlinQuery, IGremlinQuery<TElement>
    {
        
    }
}
