namespace ExRam.Gremlinq.Tests.Entities
{
    public abstract class Vertex : Element
    {
        public string PartitionKey {get; set;} = nameof(PartitionKey);
    }
}
