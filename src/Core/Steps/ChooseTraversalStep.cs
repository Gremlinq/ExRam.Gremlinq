// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = default) : base(thenTraversal, elseTraversal, ifTraversal.GetSideEffectSemanticsChange() | thenTraversal.GetSideEffectSemanticsChange() | elseTraversal.GetSideEffectSemanticsChange())
        {
            IfTraversal = ifTraversal;
        }

        public Traversal IfTraversal { get; }
    }
}
