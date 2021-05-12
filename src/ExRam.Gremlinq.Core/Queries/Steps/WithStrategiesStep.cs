namespace ExRam.Gremlinq.Core
{
    public sealed class WithStrategiesStep : Step
    {
        public WithStrategiesStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new WithStrategiesStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
