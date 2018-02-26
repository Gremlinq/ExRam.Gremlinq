using System;
using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGraphModel
    {
        IImmutableDictionary<Type, string> VertexLabels { get; }
        IImmutableDictionary<Type, string> EdgeLabels { get; }

        Option<string> IdPropertyName { get; }
    }
}