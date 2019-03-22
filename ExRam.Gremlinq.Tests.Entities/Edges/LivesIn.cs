using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Tests.Entities
{
    public class LivesIn : Edge
    {
        public Property<DateTimeOffset> Since { get; set; }
    }
}
