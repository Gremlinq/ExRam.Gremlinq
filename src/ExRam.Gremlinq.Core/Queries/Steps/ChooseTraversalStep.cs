namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = default, QuerySemantics? semantics = default) : base(thenTraversal, elseTraversal, semantics)
        {
            IfTraversal = ifTraversal;
        }

        public Traversal IfTraversal { get; }
    }
}
