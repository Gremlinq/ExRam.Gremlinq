namespace ExRam.Gremlinq.Core
{
    public sealed class WithStrategiesStep : Step
    {
        public WithStrategiesStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
