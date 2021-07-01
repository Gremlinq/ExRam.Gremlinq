namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class LocalStep : Step
    {
        public LocalStep(Traversal traversal) : base(traversal.GetTraversalSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
