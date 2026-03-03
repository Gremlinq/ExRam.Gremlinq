namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>hasKey()</c> step that filters properties by their key.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasKeyStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="HasKeyStep"/> with the specified argument.</summary>
        /// <param name="argument">The key or predicate to filter properties by.</param>
        public HasKeyStep(object argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            Argument = argument;
        }

        /// <summary>Gets the key or predicate to filter by.</summary>
        public object Argument { get; }
    }
}
