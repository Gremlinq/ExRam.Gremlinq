namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GraphSONGremlinQuery : ISerializedGremlinQuery
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
