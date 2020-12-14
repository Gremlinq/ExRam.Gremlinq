using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
