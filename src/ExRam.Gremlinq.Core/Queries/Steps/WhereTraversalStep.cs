namespace ExRam.Gremlinq.Core
{
    public sealed class WhereTraversalStep : Step, IIsOptimizableInWhere
    {
        public WhereTraversalStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
