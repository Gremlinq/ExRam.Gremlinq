using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableDictionary<Type, VertexInfo> VertexTypes { get; }
        IImmutableDictionary<Type, EdgeInfo> EdgeTypes { get; }
        IImmutableList<(Type, Type, Type)> Connections { get; }
    }
}