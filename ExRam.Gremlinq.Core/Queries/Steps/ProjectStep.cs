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

        public ProjectStep(params string[] projections)
        {
            Projections = projections;
        }

        public string[] Projections { get; }
    }
}
