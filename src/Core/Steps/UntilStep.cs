namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>until()</c> step modulator used in repeat/until/emit loop constructs.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class UntilStep : Step
    {
        public UntilStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
