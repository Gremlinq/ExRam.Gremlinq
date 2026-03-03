namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>map()</c> step that maps a traverser to a different object.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#map-step">Reference Documentation - Map Step</seealso>
    public sealed class MapStep : Step
    {
        public MapStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Traversal = traversal;
        }

        public Traversal Traversal { get; }
    }
}
