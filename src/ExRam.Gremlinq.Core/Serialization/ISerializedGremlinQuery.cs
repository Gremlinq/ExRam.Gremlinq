namespace ExRam.Gremlinq.Core.Serialization
{
    public interface ISerializedGremlinQuery
    {
        string Id { get; }

        ISerializedGremlinQuery WithNewId();
    }
}
