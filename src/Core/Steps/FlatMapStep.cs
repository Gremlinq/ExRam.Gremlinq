namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>flatMap()</c> step that maps traversers and flattens the result.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#flatmap-step">Reference Documentation - FlatMap Step</seealso>
    public sealed class FlatMapStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="FlatMapStep"/> with the specified traversal.</summary>
        /// <param name="traversal">The mapping traversal whose results are flattened.</param>
        public FlatMapStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        /// <summary>Gets the mapping traversal.</summary>
        public Traversal Traversal { get; }
    }
}
