using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGremlinSerializable
    {
        string GraphName { get; }
        IImmutableList<GremlinStep> Steps { get; }
        IIdentifierFactory IdentifierFactory { get; }
        IImmutableDictionary<MemberInfo, string> MemberInfoMappings { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IGremlinQuery<out T> : IGremlinQuery
    {

    }
}