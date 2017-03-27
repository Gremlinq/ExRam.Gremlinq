using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGraphElementNamingStrategy
    {
        Option<string> TryGetLabelOfType(IGremlinModel model, Type type);

        Option<Type> TryGetVertexTypeOfLabel(IGremlinModel model, string label);
        Option<Type> TryGetEdgeTypeOfLabel(IGremlinModel model, string label);
    }
}