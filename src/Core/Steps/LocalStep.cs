namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>local()</c> step that executes a traversal within a local scope.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#local-step">Reference Documentation - Local Step</seealso>
    public sealed class LocalStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="LocalStep"/> with the specified traversal.</summary>
        /// <param name="traversal">The traversal to execute within a local scope.</param>
        public LocalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the local traversal.</summary>
        public Traversal Traversal { get; }
    }
}
