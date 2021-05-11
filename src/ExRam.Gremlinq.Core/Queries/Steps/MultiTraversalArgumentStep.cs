using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public abstract class MultiTraversalArgumentStep : Step
    {
        protected MultiTraversalArgumentStep(ImmutableArray<Traversal> traversals, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversals = traversals;
        }

        public ImmutableArray<Traversal> Traversals { get; }
    }
}
