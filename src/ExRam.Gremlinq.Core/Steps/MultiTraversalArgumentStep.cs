using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        protected MultiTraversalArgumentStep(ImmutableArray<Traversal> traversals) : base(traversals.GetTraversalSemanticsChange())
        {
            Traversals = traversals;
        }

        public ImmutableArray<Traversal> Traversals { get; }
    }
}
