namespace ExRam.Gremlinq.Core.Serialization
{
    public sealed class GraphSONGremlinQuery : ISerializedQuery
    {
        public GraphSONGremlinQuery(string graphSON)
        {
            GraphSON = graphSON;
        }

        public string GraphSON { get; }
    }
}
