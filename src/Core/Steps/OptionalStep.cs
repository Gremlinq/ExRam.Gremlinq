namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>optional()</c> step that returns the traversal result if available, otherwise the original traverser.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#optional-step">Reference Documentation - Optional Step</seealso>
    public sealed class OptionalStep : Step
    {
        public OptionalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
