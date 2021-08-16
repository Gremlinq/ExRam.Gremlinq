namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = default) : base(thenTraversal, elseTraversal, ifTraversal.GetSideEffectSemanticsChange() | thenTraversal.GetSideEffectSemanticsChange() | elseTraversal.GetSideEffectSemanticsChange())
        {
            IfTraversal = ifTraversal;
        }

        public Traversal IfTraversal { get; }
    }
}
