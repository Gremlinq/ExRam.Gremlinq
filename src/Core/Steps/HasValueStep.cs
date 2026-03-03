namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>hasValue()</c> step that filters properties by their value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasValueStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="HasValueStep"/> with the specified argument.</summary>
        /// <param name="argument">The value or predicate to filter properties by.</param>
        public HasValueStep(object argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            Argument = argument;
        }

        /// <summary>Gets the value or predicate to filter by.</summary>
        public object Argument { get; }
    }
}
