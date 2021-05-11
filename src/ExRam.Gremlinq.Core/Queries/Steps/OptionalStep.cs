namespace ExRam.Gremlinq.Core
{
    public sealed class OptionalStep : Step
    {
        public OptionalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
