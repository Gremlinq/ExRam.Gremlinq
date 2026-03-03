namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>not()</c> step that negates a filter traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#not-step">Reference Documentation - Not Step</seealso>
    public sealed class NotStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="NotStep"/> with the specified filter traversal.</summary>
        /// <param name="traversal">The traversal whose result is negated.</param>
        public NotStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the filter traversal whose result is negated.</summary>
        public Traversal Traversal { get; }
    }
}
