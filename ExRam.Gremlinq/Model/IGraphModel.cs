using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableList<VertexInfo> VertexTypes { get; }
        IImmutableList<EdgeInfo> EdgeTypes { get; }
        IImmutableList<(Type, Type, Type)> Connections { get; }
    }
}