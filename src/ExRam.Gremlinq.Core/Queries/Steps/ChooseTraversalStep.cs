namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = default, QuerySemantics? semantics = default) : base(thenTraversal, elseTraversal, semantics)
        {
            IfTraversal = ifTraversal;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ChooseTraversalStep(IfTraversal, ThenTraversal, ElseTraversal, semantics);

        public Traversal IfTraversal { get; }
    }
}
