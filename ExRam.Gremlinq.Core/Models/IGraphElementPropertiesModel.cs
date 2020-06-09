using System;
using System.Collections.Immutable;
using System.Reflection;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel ConfigureMemberMetadata(Func<IImmutableDictionary<MemberInfo, PropertyMetadata>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation);

        IGraphElementPropertyModel ConfigureSpecialNames(Func<IImmutableDictionary<MemberInfo, T>, IImmutableDictionary<MemberInfo, T>> transformation);

        IImmutableDictionary<MemberInfo, PropertyMetadata> MemberMetadata { get; }

        IImmutableDictionary<MemberInfo, T> SpecialNames { get; }
    }
}
