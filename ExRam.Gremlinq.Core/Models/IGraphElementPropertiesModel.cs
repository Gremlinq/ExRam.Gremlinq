using System;
using System.Collections.Immutable;
using System.Reflection;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyModel
    {
        IGraphElementPropertyModel ConfigureMetadata(Func<IImmutableDictionary<MemberInfo, PropertyMetadata>, IImmutableDictionary<MemberInfo, PropertyMetadata>> transformation);

        IGraphElementPropertyModel ConfigureSpecialNames(Func<IImmutableDictionary<string, T>, IImmutableDictionary<string, T>> transformation);

        IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata { get; }

        IImmutableDictionary<string, T> SpecialNames { get; }
    }
}
