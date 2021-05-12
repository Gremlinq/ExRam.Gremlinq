namespace ExRam.Gremlinq.Core
{
    public sealed class FlatMapStep : Step
    {
        public FlatMapStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new FlatMapStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
