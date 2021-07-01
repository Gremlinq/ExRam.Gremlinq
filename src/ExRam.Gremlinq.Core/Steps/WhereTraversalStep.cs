namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class WhereTraversalStep : Step, IIsOptimizableInWhere
    {
        public WhereTraversalStep(Traversal traversal) : base(traversal.GetTraversalSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
