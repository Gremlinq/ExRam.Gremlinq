namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Vertex : Element
    {
        protected Vertex()
        {
            PartitionKey = nameof(PartitionKey);
        }
    }
}
