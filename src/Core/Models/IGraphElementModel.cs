using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementModel
    {
        IGraphElementModel AddAssemblies(params Assembly[] assemblies);

        IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        IGraphElementModel ConfigureMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation);

        MemberMetadata? TryGetMetadata(MemberInfo memberInfo);

        ElementMetadata? TryGetMetadata(Type elementType);

        IImmutableSet<Type> ElementTypes { get; }

        IImmutableSet<MemberInfo> Members { get; }
    }
}
