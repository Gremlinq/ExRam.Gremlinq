using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class GraphSONGremlinQuery : ISerializedGremlinQuery
    {
        public GraphSONGremlinQuery(string graphSON) : this(Guid.NewGuid().ToString(), graphSON)
        {
        }

        public GraphSONGremlinQuery(string queryId, string graphSON)
        {
            Id = queryId;
            GraphSON = graphSON;
        }

        public string Id { get; }

        public string GraphSON { get; }

        public ISerializedGremlinQuery WithNewId() => new GraphSONGremlinQuery(GraphSON);

    }
}
