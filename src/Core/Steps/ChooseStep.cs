namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for Gremlin <c>choose()</c> steps that route traversers based on conditions.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#choose-step">Reference Documentation - Choose Step</seealso>
    public abstract class ChooseStep : Step
    {
        protected ChooseStep(Traversal thenTraversal, Traversal? elseTraversal = null, SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.Write) : base(sideEffectSemanticsChange)
        {
            ThenTraversal = thenTraversal;
            ElseTraversal = elseTraversal;
        }

        public Traversal ThenTraversal { get; }

        public Traversal? ElseTraversal { get; }
    }
}
