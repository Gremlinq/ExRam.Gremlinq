using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementModel
    {
        ElementMetadata GetMetadata(Type elementType);

        IGraphElementModel AddAssemblies(params Assembly[] assemblies);

        IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        IImmutableSet<Type> ElementTypes { get; }
    }
}
