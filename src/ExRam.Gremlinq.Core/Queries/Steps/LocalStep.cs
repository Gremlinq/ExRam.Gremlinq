namespace ExRam.Gremlinq.Core
{
    public sealed class LocalStep : Step
    {
        public LocalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
