namespace ExRam.Gremlinq.Core
{
    public sealed class GroupStep : Step
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

        private GroupStep()
        {

        }

        public static readonly GroupStep Instance = new GroupStep();
    }
}
