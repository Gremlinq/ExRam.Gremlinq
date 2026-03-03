using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>match()</c> step that pattern-matches against a set of traversals.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#match-step">Reference Documentation - Match Step</seealso>
    public sealed class MatchStep : MultiTraversalArgumentStep
    {
        public MatchStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
