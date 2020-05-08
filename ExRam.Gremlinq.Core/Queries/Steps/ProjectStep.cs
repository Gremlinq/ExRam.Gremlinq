using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public sealed class ByTraversalStep : Step
        {
            public ByTraversalStep(Traversal traversal)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
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
