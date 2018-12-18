namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class Authority : Vertex
    {
        public string Name { get; set; }
        public string[] PhoneNumbers { get; set; }
    }
}
