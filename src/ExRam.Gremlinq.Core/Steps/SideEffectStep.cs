namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal) : base(traversal.GetTraversalSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
