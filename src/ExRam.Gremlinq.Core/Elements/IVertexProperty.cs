namespace ExRam.Gremlinq.Core.GraphElements
{
    internal interface IVertexProperty
    {
        IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment);
    }
}
