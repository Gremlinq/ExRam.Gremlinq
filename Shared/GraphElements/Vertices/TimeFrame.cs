using System;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TimeFrame : Vertex
    {
        public bool Enabled { get; set; }

        public virtual TimeSpan StartTime { get; set; }

        public virtual TimeSpan Duration { get; set; }
    }
}
