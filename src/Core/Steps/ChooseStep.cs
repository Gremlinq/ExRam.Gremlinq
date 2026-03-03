namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for Gremlin <c>choose()</c> steps that route traversers based on conditions.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
    public abstract class ChooseStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="ChooseStep"/>.</summary>
        /// <param name="thenTraversal">The traversal to execute when the condition is true.</param>
        /// <param name="elseTraversal">The optional traversal to execute when the condition is false.</param>
        /// <param name="sideEffectSemanticsChange">The side-effect semantics change.</param>
        protected ChooseStep(Traversal thenTraversal, Traversal? elseTraversal = null, SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.Write) : base(sideEffectSemanticsChange)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        /// <summary>Gets the traversal executed when the condition is true.</summary>
        public Traversal ThenTraversal { get; }

        /// <summary>Gets the optional traversal executed when the condition is false.</summary>
        public Traversal? ElseTraversal { get; }
    }
}
