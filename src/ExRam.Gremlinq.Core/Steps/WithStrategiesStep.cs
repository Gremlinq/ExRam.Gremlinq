namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class WithStrategiesStep : Step
    {
        public WithStrategiesStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
