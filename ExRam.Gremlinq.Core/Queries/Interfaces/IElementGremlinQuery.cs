namespace ExRam.Gremlinq.Core
{
    public partial interface IElementGremlinQuery : IGremlinQuery
    {
        IValueGremlinQuery<object> Id();

        IValueGremlinQuery<string> Label();

        IValueGremlinQuery<TTarget> Values<TTarget>(params string[] keys);
    }

    public partial interface IElementGremlinQuery<TElement> : IElementGremlinQuery, IGremlinQuery<TElement>
    {
        
    }
}
