namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(Traversal thenTraversal, Traversal? elseTraversal = default, SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.Write) : base(sideEffectSemanticsChange)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public Traversal ThenTraversal { get; }

        public Traversal? ElseTraversal { get; }
    }
}
