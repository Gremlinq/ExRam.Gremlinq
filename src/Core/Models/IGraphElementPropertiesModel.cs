using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel ConfigureMemberMetadata(MemberInfo member, Func<MemberMetadata, MemberMetadata> transformation);

        IGraphElementPropertyModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        IGraphElementPropertyModel AddType(Type type);

        MemberMetadata GetMetadata(MemberInfo memberInfo);

        IImmutableSet<MemberInfo> Members { get; }
    }
}
