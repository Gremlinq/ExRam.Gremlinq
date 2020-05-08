namespace ExRam.Gremlinq.Core
{
    public sealed class WithStrategiesStep : Step
    {
        public WithStrategiesStep(Traversal traversal)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
