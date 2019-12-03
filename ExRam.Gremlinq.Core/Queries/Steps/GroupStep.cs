namespace ExRam.Gremlinq.Core
{
    public sealed class GroupStep : Step
    {
        public sealed class ByTraversalStep : Step
        {
            public IGremlinQuery Traversal { get; }

            public ByTraversalStep(IGremlinQuery traversal)
            {
                Traversal = traversal;
            }
        }

        private GroupStep()
        {

        }

        public static readonly GroupStep Instance = new GroupStep();
    }
}