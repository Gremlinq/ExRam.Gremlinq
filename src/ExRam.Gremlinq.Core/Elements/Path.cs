using System;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public sealed class Path
    {
        public string[][] Labels { get; set; } = Array.Empty<string[]>();
        public object[] Objects { get; set; } = Array.Empty<object>();
    }
}
