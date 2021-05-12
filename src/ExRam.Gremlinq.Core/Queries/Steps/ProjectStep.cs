using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep(QuerySemantics? semantics = default) : base(semantics)
            {
            }
        }

        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
            }

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ByTraversalStep(Traversal, semantics);

            public Traversal Traversal { get; }
        }

        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key, QuerySemantics? semantics = default) : base(semantics)
            {
                Key = key;
            }

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ByKeyStep(Key, semantics);

            public Key Key { get; }
        }

        public ProjectStep(ImmutableArray<string> projections, QuerySemantics? semantics = default) : base(semantics)
        {
            Projections = projections;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ProjectStep(Projections, semantics);

        public ImmutableArray<string> Projections { get; }
    }
}
