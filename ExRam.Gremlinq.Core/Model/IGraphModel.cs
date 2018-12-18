using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        Option<string> TryGetLabel(Type elementType);
        Option<Type> TryGetType(string label);
        string[] TryGetDerivedLabels(Type elementType);

        Option<string> VertexIdPropertyName { get; }
        Option<string> EdgeIdPropertyName { get; }
    }
}
