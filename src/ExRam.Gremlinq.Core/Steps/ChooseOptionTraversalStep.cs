namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ChooseOptionTraversalStep : Step
    {
        public ChooseOptionTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
