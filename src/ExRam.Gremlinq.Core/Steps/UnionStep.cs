using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class UnionStep : MultiTraversalArgumentStep
    {
        public UnionStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
