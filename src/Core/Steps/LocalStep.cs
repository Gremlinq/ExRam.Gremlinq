namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>local()</c> step that executes a traversal within a local scope.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#local-step">Reference Documentation - Local Step</seealso>
    public sealed class LocalStep : Step
    {
        public LocalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
