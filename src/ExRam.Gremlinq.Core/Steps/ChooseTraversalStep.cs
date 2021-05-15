namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = default) : base(thenTraversal, elseTraversal)
        {
            IfTraversal = ifTraversal;
        }

        public Traversal IfTraversal { get; }
    }
}
