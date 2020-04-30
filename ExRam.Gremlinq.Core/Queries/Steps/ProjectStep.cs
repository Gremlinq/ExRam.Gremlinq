using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public sealed class ByTraversalStep : SingleTraversalArgumentStep
        {
            public ByTraversalStep(Traversal traversal) : base(traversal)
            {
            }
        }

        public sealed class ByKeyStep : Step
        {
            public ByKeyStep(object key)
            {
                Key = key;
            }

            public object Key { get; }
        }

        public ProjectStep(ImmutableArray<string> projections)
        {
            Projections = projections;
        }

        public ImmutableArray<string> Projections { get; }
    }
}
