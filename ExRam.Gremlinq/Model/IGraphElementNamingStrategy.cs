using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGraphElementNamingStrategy
    {
        Option<string> TryGetLabelOfType(IGremlinModel model, Type type);
        Option<Type> TryGetTypeOfLabel(IGremlinModel model, string label);
    }
}