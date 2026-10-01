namespace ExRam.Gremlinq.Tests.Entities
{
    public class RichEdge : Edge
    {
        public string? Role { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }
    }
}
