using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGremlinSerializable
    {
        string TraversalSourceName { get; }
        IImmutableList<GremlinStep> Steps { get; }
        IIdentifierFactory IdentifierFactory { get; }
        //(int depth, int index) TreeLocation { get; }
        IImmutableDictionary<string, StepLabel> StepLabelMappings { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IGremlinQuery<out T> : IGremlinQuery
    {

    }
}