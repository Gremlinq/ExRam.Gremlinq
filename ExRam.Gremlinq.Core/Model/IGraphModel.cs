using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Option<string> TryGetConstructiveLabel(Type elementType);
        Option<string[]> TryGetFilterLabels(Type elementType);

        Type[] GetTypes(string label);

        Option<string> VertexIdPropertyName { get; }
        Option<string> EdgeIdPropertyName { get; }
    }
}
