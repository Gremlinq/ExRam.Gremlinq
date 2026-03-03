namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>repeat()</c> step that defines a looping traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class RepeatStep : Step
    {
        public RepeatStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
