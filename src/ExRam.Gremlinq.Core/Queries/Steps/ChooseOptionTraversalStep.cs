namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseOptionTraversalStep : Step
    {
        public ChooseOptionTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
