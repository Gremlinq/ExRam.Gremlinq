using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        protected MultiTraversalArgumentStep(ImmutableArray<Traversal> traversals) : base()
        {
            Traversals = traversals;
        }

        public ImmutableArray<Traversal> Traversals { get; }
    }
}
