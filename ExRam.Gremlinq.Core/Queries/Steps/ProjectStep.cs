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
        }

        public ProjectStep(params string[] projections)
        {
            Projections = projections;
        }

        public string[] Projections { get; }
    }
}
