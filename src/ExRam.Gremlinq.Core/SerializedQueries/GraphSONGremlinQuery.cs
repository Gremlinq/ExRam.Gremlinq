namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GraphSONGremlinQuery : ISerializedQuery
    {
        public GraphSONGremlinQuery(string queryId, string graphSON)
        {
            Id = queryId;
            GraphSON = graphSON;
        }

        public string Id { get; }

        public string GraphSON { get; }
    }
}
