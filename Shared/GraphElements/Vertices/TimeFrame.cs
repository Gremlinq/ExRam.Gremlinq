using System;
using ExRam.Gremlinq.GraphElements;

namespace ExRam.Gremlinq.Tests
{
    public class TimeFrame : Vertex
    {
        public virtual DayOfWeek WeekDay { get; set; }

        public virtual TimeSpan StartTime { get; set; }

        public virtual TimeSpan Duration { get; set; }

        public bool Enabled { get; set; }
    }
}
