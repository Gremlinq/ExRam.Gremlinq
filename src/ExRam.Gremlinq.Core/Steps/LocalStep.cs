namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class LocalStep : Step
    {
        public LocalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
