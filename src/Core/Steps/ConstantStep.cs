namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>constant()</c> step that maps the traverser to a constant value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#constant-step">Reference Documentation - Constant Step</seealso>
    public sealed class ConstantStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="ConstantStep"/> with the specified constant value.</summary>
        /// <param name="value">The constant value to map traversers to.</param>
        public ConstantStep(object? value)
        {
            Value = value;
        }

        /// <summary>Gets the constant value.</summary>
        public object? Value { get; }
    }
}
