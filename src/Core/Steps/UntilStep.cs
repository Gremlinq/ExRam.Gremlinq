namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>until()</c> step modulator used in repeat/until/emit loop constructs.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class UntilStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="UntilStep"/> with the specified condition traversal.</summary>
        /// <param name="traversal">The traversal that determines when to stop looping.</param>
        public UntilStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the condition traversal.</summary>
        public Traversal Traversal { get; }
    }
}
