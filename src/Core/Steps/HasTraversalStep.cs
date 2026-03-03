namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>has(key, traversal)</c> step that filters elements by a property traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasTraversalStep : Step, IFilterStep
    { 
        public HasTraversalStep(Key key, Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Key = key;
            Traversal = traversal;
        }

        public Key Key { get; }
        public Traversal Traversal { get; }
    }
}
