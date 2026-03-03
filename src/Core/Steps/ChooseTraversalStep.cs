// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents a Gremlin <c>choose()</c> step with a traversal-based condition.</summary>
    public sealed class ChooseTraversalStep : ChooseStep
    {
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = null) : base(thenTraversal, elseTraversal, ifTraversal.GetSideEffectSemanticsChange() | thenTraversal.GetSideEffectSemanticsChange() | elseTraversal.GetSideEffectSemanticsChange())
        {
            IfTraversal = ifTraversal;
        }

        public Traversal IfTraversal { get; }
    }
}
