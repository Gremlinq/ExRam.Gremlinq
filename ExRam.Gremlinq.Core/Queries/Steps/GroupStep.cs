namespace ExRam.Gremlinq.Core
{
    public sealed class GroupStep : Step
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

        public static readonly GroupStep Instance = new GroupStep();

        private GroupStep()
        {

        }
    }
}
