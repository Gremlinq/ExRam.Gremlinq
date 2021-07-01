namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class UntilStep : Step
    {
        public UntilStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
