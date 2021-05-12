namespace ExRam.Gremlinq.Core
{
    public sealed class UntilStep : Step
    {
        public UntilStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new UntilStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
