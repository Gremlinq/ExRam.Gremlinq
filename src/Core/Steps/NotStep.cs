namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>not()</c> step that negates a filter traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#not-step">Reference Documentation - Not Step</seealso>
    public sealed class NotStep : Step, IFilterStep
    {
        public NotStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
