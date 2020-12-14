using System;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class TimeFrame : Vertex
    {
        public bool Enabled { get; set; }

        public virtual TimeSpan StartTime { get; set; }

        public virtual TimeSpan Duration { get; set; }
    }
}
