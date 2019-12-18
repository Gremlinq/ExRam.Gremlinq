namespace ExRam.Gremlinq.Core
{
    public sealed class GroupStep : Step
    {
        public sealed class ByTraversalStep : SingleTraversalArgumentStep
        {
            public ByTraversalStep(IGremlinQueryBase traversal) : base(traversal)
            {
            }
        }

        private GroupStep()
        {

        }

        public static readonly GroupStep Instance = new GroupStep();
    }
}
