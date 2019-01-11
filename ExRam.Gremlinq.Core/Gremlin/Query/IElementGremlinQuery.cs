namespace ExRam.Gremlinq.Core
{
    public interface IElementGremlinQuery : IGremlinQuery
    {
        IGremlinQuery<object> Id();
    }
}