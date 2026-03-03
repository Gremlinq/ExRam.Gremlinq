using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>union()</c> step that merges results of multiple traversals.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#union-step">Reference Documentation - Union Step</seealso>
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
