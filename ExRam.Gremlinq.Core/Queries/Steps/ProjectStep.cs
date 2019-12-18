namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public sealed class ByTraversalStep : SingleTraversalArgumentStep
        {
            public ByTraversalStep(IGremlinQueryBase traversal) : base(traversal)
            {
            }
        }

        public ProjectStep(params string[] projections)
        {
            Projections = projections;
        }

        public string[] Projections { get; }
    }
}
