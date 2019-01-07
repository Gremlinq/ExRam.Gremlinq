using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Option<string> TryGetConstructiveVertexLabel(Type elementType);
        Option<string> TryGetConstructiveEdgeLabel(Type elementType);

        Option<string[]> TryGetVertexFilterLabels(Type elementType);
        Option<string[]> TryGetEdgeFilterLabels(Type elementType);

        Type[] GetTypes(string label);

        Option<string> VertexIdPropertyName { get; }
        Option<string> EdgeIdPropertyName { get; }
    }
}
