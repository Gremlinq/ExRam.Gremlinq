namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class NotStep : Step, IIsOptimizableInWhere
    {
        public NotStep(Traversal traversal) : base(traversal.GetTraversalSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
