using System;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        Option<string> TryGetLabel(Type elementType);
        Option<Type> TryGetType(string label);
        string[] TryGetDerivedLabels(Type elementType);

        //IImmutableDictionary<Type, string> VertexLabels { get; }
        //IImmutableDictionary<Type, string> EdgeLabels { get; }

        Option<string> IdPropertyName { get; }
    }
}
