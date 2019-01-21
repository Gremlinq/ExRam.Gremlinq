using System;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Samples
{
    public class NameMeta
    {
        public string Creator { get; set; }
        public DateTimeOffset Date { get; set; }
    }

    public class Person : Vertex
    {
        public int Age { get; set; }

        public VertexProperty<string, NameMeta> Name { get; set; }
    }
}