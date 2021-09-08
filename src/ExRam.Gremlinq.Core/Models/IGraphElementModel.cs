using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementModel
    {
        IImmutableDictionary<Type, ElementMetadata> Metadata { get; }

        IGraphElementModel ConfigureMetadata(Func<IImmutableDictionary<Type, ElementMetadata>, IImmutableDictionary<Type, ElementMetadata>> transformation);
    }
}
