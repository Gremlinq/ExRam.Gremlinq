// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents a Gremlin <c>choose()</c> step with a traversal-based condition.</summary>
    public sealed class ChooseTraversalStep : ChooseStep
    {
        /// <summary>Initializes a new instance of <see cref="ChooseTraversalStep"/>.</summary>
        /// <param name="ifTraversal">The condition traversal.</param>
        /// <param name="thenTraversal">The traversal to execute when the condition is true.</param>
        /// <param name="elseTraversal">The optional traversal to execute when the condition is false.</param>
        public ChooseTraversalStep(Traversal ifTraversal, Traversal thenTraversal, Traversal? elseTraversal = null) : base(thenTraversal, elseTraversal, ifTraversal.GetSideEffectSemanticsChange() | thenTraversal.GetSideEffectSemanticsChange() | (elseTraversal?.GetSideEffectSemanticsChange() ?? SideEffectSemanticsChange.None))
        {
            IfTraversal = ifTraversal;
        }

        /// <summary>Gets the condition traversal.</summary>
        public Traversal IfTraversal { get; }
    }
}
