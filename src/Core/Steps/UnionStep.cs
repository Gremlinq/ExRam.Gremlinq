using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>union()</c> step that merges results of multiple traversals.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#union-step">Reference Documentation - Union Step</seealso>
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        /// <summary>Initializes a new instance of <see cref="UnionStep"/> with the specified traversals.</summary>
        /// <param name="traversals">The traversals whose results are merged.</param>
        public UnionStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
