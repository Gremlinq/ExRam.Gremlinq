using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public sealed class ByTraversalStep : Step
        {
            public IGremlinQuery Traversal { get; }

            public ByTraversalStep(IGremlinQuery traversal)
            {
                Traversal = traversal;
            }

            public override void Accept(IGremlinQueryElementVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public ProjectStep(params string[] projections)
        {
            Projections = projections;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string[] Projections { get; }
    }
}
