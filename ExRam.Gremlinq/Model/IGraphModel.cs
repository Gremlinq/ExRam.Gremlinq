using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableDictionary<Type, string> VertexLabels { get; }
        IImmutableDictionary<Type, string> EdgeLabels { get; }
    }
}