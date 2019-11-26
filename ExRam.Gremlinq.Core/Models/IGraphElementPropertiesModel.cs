using System.Collections.Immutable;
using System.Reflection;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IGraphElementPropertyModel
    {
        IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata { get; }

        IImmutableDictionary<string, T> SpecialNames { get; }
    }
}
