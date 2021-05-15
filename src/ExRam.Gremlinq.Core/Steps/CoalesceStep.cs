using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class CoalesceStep : MultiTraversalArgumentStep
    {
        public CoalesceStep(ImmutableArray<Traversal> traversals) : base(traversals)
        {
        }
    }
}
