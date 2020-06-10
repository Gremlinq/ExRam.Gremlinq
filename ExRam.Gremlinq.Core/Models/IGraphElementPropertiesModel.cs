using System;
using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation);

        IImmutableDictionary<MemberInfo, MemberMetadata> MemberMetadata { get; }
    }
}
