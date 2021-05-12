namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseOptionTraversalStep : Step
    {
        public ChooseOptionTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Traversal = traversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ChooseOptionTraversalStep(Traversal, semantics);

        public Traversal Traversal { get; }
    }
}
