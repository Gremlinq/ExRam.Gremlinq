namespace ExRam.Gremlinq.Templates.AspNet
{
    public abstract class Mammal : Vertex
    {
        public int? Age { get; set; }

        public string? Name { get; set; }
    }
}
