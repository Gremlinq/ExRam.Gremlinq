namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>has(key, traversal)</c> step that filters elements by a property traversal.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasTraversalStep : Step, IFilterStep
    { 
        /// <summary>Initializes a new instance of <see cref="HasTraversalStep"/> with the specified key and traversal.</summary>
        /// <param name="key">The property key to filter on.</param>
        /// <param name="traversal">The traversal the property value must satisfy.</param>
        public HasTraversalStep(Key key, Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
        {
            Key = key;
            Traversal = traversal;
        }

        /// <summary>Gets the property key.</summary>
        public Key Key { get; }
        /// <summary>Gets the filter traversal.</summary>
        public Traversal Traversal { get; }
    }
}
