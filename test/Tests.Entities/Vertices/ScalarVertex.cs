namespace ExRam.Gremlinq.Tests.Entities
{
    public class ScalarVertex : Vertex
    {
        public bool Enabled { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
