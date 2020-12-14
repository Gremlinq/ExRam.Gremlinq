using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        protected MultiTraversalArgumentStep(ImmutableArray<Traversal> traversals)
        {
            Traversals = traversals;
        }

        public ImmutableArray<Traversal> Traversals { get; }
    }
}
