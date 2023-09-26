using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel AddType(Type type);

        IGraphElementPropertyModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation);

        IGraphElementPropertyModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        MemberMetadata? TryGetMetadata(MemberInfo memberInfo);

        IImmutableSet<MemberInfo> Members { get; }
    }
}
