namespace ExRam.Gremlinq.Core
{
    public sealed class OptionalStep : Step
    {
        public OptionalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new OptionalStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
