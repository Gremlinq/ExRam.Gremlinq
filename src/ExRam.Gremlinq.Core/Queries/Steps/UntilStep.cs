namespace ExRam.Gremlinq.Core
{
    public sealed class UntilStep : Step
    {
        public UntilStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
