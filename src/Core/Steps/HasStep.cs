namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>has(key)</c> step that filters elements possessing a given property.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="HasStep"/> with the specified property key.</summary>
        /// <param name="key">The property key to check for existence.</param>
        public HasStep(Key key)
        {
            Key = key;
        }

        /// <summary>Gets the property key.</summary>
        public Key Key { get; }
    }
}
