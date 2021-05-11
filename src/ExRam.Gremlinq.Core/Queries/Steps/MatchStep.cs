using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class MatchStep : MultiTraversalArgumentStep
    {
        public MatchStep(ImmutableArray<Traversal> traversals, QuerySemantics? semantics = default) : base(traversals, semantics)
        {
        }
    }
}
