using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core.Tests
{
    public class LivesIn : Edge
    {
        public Property<DateTimeOffset> Since { get; set; }
    }
}
