namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>hasKey()</c> step that filters properties by their key.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasKeyStep : Step, IFilterStep
    {
        public HasKeyStep(object argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            Argument = argument;
        }

        public object Argument { get; }
    }
}
