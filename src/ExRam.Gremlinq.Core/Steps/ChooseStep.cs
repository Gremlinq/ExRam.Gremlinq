namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(Traversal thenTraversal, Traversal? elseTraversal = default, TraversalSemanticsChange traversalSemanticsChange = TraversalSemanticsChange.Write) : base(traversalSemanticsChange)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public Traversal ThenTraversal { get; }

        public Traversal? ElseTraversal { get; }
    }
}
