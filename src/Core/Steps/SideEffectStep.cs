namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
