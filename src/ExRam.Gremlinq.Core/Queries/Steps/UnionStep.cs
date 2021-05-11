using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(ImmutableArray<Traversal> traversals, QuerySemantics? semantics = default) : base(traversals, semantics)
        {
        }
    }
}
