using System;
using System.Collections.Immutable;
using System.Reflection;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, MemberMetadata>, IImmutableDictionary<MemberInfo, MemberMetadata>> transformation);

        IGraphElementPropertyModel ConfigureSpecialNames(Func<IImmutableDictionary<MemberInfo, T>, IImmutableDictionary<MemberInfo, T>> transformation);

        IImmutableDictionary<MemberInfo, MemberMetadata> MemberMetadata { get; }

        IImmutableDictionary<MemberInfo, T> SpecialNames { get; }
    }
}
