using System;
using System.Collections.Immutable;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementModel
    {
        Option<string[]> TryGetFilterLabels(Type elementType);

        IImmutableDictionary<Type, string> Labels { get; }
    }
}
