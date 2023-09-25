using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core.Models
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel ConfigureMemberMetadata(Func<MemberInfo, MemberMetadata, MemberMetadata> transformation);

        IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation);

        MemberMetadata GetMetadata(MemberInfo memberInfo);

        IImmutableSet<MemberInfo> Members { get; }
    }
}
