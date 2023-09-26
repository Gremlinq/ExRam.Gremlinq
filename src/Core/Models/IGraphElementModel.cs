using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementModel
    {
        IGraphElementModel AddAssemblies(params Assembly[] assemblies);

        ElementMetadata GetMetadata(Type elementType);

        IGraphElementModel ConfigureMetadata(Func<Type, ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureMetadata(Type elementType, Func<ElementMetadata, ElementMetadata> metaDataTransformation);

        IGraphElementModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation);

        IGraphElementModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        MemberMetadata? TryGetMetadata(MemberInfo memberInfo);

        IImmutableSet<Type> ElementTypes { get; }

        IImmutableSet<MemberInfo> Members { get; }
    }
}
