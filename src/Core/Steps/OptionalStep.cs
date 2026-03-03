namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>optional()</c> step that returns the traversal result if available, otherwise the original traverser.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#optional-step">Reference Documentation - Optional Step</seealso>
    public sealed class OptionalStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="OptionalStep"/> with the specified traversal.</summary>
        /// <param name="traversal">The optional traversal.</param>
        public OptionalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the optional traversal.</summary>
        public Traversal Traversal { get; }
    }
}
