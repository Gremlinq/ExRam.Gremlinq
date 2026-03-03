using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>coalesce()</c> step that evaluates traversals in order and returns the first with results.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#coalesce-step">Reference Documentation - Coalesce Step</seealso>
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
