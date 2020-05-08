namespace ExRam.Gremlinq.Core
{
    public sealed class WhereTraversalStep : Step
    {
        public WhereTraversalStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
