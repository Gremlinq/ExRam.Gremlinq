using System;
using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementModel
    {
        Type[] GetTypes(string label);
        Option<string> TryGetConstructiveLabel(Type elementType);
        Option<string[]> TryGetFilterLabels(Type elementType);

        ImmutableDictionary<Type, string> Labels { get; }
    }
}
