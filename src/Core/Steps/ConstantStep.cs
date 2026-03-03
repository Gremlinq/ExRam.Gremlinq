namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>constant()</c> step that maps the traverser to a constant value.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#constant-step">Reference Documentation - Constant Step</seealso>
    public sealed class ConstantStep : Step
    {
        public ConstantStep(object? value)
        {
            Value = value;
        }

        public object? Value { get; }
    }
}
