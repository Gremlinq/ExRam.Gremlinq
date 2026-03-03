namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>flatMap()</c> step that maps traversers and flattens the result.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#flatmap-step">Reference Documentation - FlatMap Step</seealso>
    public sealed class FlatMapStep : Step
    {
        public FlatMapStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
