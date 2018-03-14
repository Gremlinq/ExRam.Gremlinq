using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGremlinSerializable
    {
        string TraversalSourceName { get; }
        IImmutableList<GremlinStep> Steps { get; }
        IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IGremlinQuery<out TElement> : IGremlinQuery
    {

    }

    // ReSharper disable once UnusedTypeParameter
    public interface IGremlinQuery<out TEdge, out TAdjacentVertex> : IGremlinQuery<TEdge>
    {

    }

    // ReSharper disable UnusedTypeParameter
    public interface IGremlinQuery<out TOutVertex, out TEdge, out TInVertex> : IGremlinQuery<TEdge>
    // ReSharper restore UnusedTypeParameter
    {

    }
}