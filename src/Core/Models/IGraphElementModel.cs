using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementModel
    {
        IImmutableSet<Type> ElementTypes { get; }

        ElementMetadata GetMetadata(Type elementType);

        IGraphElementModel AddAssemblies(params Assembly[] assemblies);

        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureLabels(Func<Type, string, string> overrideTransformation);
    }
}
