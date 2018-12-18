using System;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphModel
    {
        string[] GetLabels(Type elementType, bool includeDerivedTypes = false);
        Type[] GetTypes(string label);

        Option<string> VertexIdPropertyName { get; }
        Option<string> EdgeIdPropertyName { get; }
    }
}
