using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementModel
    {
        IImmutableDictionary<Type, ElementMetadata> Metadata { get; }

        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation);
    }
}
