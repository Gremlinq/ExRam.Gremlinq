namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>sideEffect()</c> step that executes a side-effect traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#sideeffect-step">Reference Documentation - SideEffect Step</seealso>
    public sealed class SideEffectStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="SideEffectStep"/> with the specified traversal.</summary>
        /// <param name="traversal">The side-effect traversal.</param>
        public SideEffectStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the side-effect traversal.</summary>
        public Traversal Traversal { get; }
    }
}
