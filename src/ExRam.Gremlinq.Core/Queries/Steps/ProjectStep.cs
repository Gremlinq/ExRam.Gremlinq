using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public abstract class ByStep : Step
        {

        }

        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            public Key Key { get; }
        }

        public static readonly ProjectStep Empty = new ProjectStep(ImmutableArray<string>.Empty);

        public ProjectStep(ImmutableArray<string> projections)
        {
            Projections = projections;
        }

        public ImmutableArray<string> Projections { get; }
    }
}
