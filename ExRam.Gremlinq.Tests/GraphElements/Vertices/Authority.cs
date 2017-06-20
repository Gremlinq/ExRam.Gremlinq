namespace ExRam.Gremlinq.Tests
{
    public abstract class Authority : Vertex
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}