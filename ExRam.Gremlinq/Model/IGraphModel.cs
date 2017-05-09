using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableList<Type> VertexTypes { get; }
        IImmutableList<Type> EdgeTypes { get; }
    }
}