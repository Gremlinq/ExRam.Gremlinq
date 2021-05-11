namespace ExRam.Gremlinq.Core
{
    public sealed class FlatMapStep : Step
    {
        public FlatMapStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
