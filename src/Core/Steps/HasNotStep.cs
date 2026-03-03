namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>hasNot()</c> step that filters elements lacking a given property.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasNotStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="HasNotStep"/> with the specified property key.</summary>
        /// <param name="key">The property key to check for absence.</param>
        public HasNotStep(Key key)
        {
            Key = key;
        }

        /// <summary>Gets the property key.</summary>
        public Key Key { get; }
    }
}
