using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ConcatTraversalsStep : Step
    {
        public ConcatTraversalsStep(ImmutableArray<Traversal> traversals)
        {
            Traversals = traversals;
        }

        public ImmutableArray<Traversal> Traversals { get; }
    }
}
