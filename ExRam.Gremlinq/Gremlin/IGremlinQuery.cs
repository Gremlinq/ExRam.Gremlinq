using System.Collections.Immutable;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGremlinSerializable
    {
        string GraphName { get; }
        IGremlinQueryProvider Provider { get; }
        IImmutableList<TerminalGremlinStep> Steps { get; }
        IIdentifierFactory IdentifierFactory { get; }
        IImmutableDictionary<MemberInfo, string> MemberInfoMappings { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IGremlinQuery<out T> : IGremlinQuery
    {

    }
}