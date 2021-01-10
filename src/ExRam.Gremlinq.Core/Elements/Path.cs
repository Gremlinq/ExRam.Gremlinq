using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public sealed class Path
    {
        public string[][]? Labels { get; set; }
        public object[]? Objects { get; set; }
    }
}
