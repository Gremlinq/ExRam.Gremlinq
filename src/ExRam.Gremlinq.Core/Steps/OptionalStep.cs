namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OptionalStep : Step
    {
        public OptionalStep(Traversal traversal) : base(traversal.GetTraversalSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
