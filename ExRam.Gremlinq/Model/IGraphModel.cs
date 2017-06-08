using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableDictionary<Type, VertexTypeInfo> VertexTypes { get; }
        IImmutableDictionary<Type, EdgeTypeInfo> EdgeTypes { get; }
        IImmutableList<(Type, Type, Type)> Connections { get; }
    }
}