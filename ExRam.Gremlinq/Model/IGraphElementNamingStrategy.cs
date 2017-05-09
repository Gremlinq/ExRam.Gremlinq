using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGraphElementNamingStrategy
    {
        Option<string> TryGetLabelOfType(IGraphModel model, Type type);

        Option<Type> TryGetVertexTypeOfLabel(IGraphModel model, string label);
        Option<Type> TryGetEdgeTypeOfLabel(IGraphModel model, string label);
    }
}