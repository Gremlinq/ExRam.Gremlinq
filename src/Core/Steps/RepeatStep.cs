namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>repeat()</c> step that defines a looping traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class RepeatStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="RepeatStep"/> with the specified loop traversal.</summary>
        /// <param name="traversal">The traversal to repeat.</param>
        public RepeatStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the loop traversal.</summary>
        public Traversal Traversal { get; }
    }
}
