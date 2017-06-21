using System;

namespace ExRam.Gremlinq.Tests
{
    public class WorksFor : Edge
    {
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}