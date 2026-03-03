namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>sideEffect()</c> step that executes a side-effect traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#sideeffect-step">Reference Documentation - SideEffect Step</seealso>
    public sealed class SideEffectStep : Step
    {
        public SideEffectStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
