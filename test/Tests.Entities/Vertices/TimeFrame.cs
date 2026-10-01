namespace ExRam.Gremlinq.Tests.Entities
{
    public class TimeFrame : Vertex
    {
        public bool Enabled { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
