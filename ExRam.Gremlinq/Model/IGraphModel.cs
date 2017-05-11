using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableList<GraphElementInfo> VertexTypes { get; }
        IImmutableList<GraphElementInfo> EdgeTypes { get; }
        IImmutableList<(GraphElementInfo, GraphElementInfo, GraphElementInfo)> Connections { get; }
    }
}