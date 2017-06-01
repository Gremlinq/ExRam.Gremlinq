using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableList<VertexInfo> VertexTypes { get; }
        IImmutableList<EdgeInfo> EdgeTypes { get; }
        IImmutableList<(VertexInfo, EdgeInfo, VertexInfo)> Connections { get; }
    }
}