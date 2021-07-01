namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class RepeatStep : Step
    {
        public RepeatStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
