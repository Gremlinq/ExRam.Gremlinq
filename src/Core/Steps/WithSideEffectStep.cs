namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>withSideEffect()</c> source step that registers a side-effect on the traversal source.</summary>
    public sealed class WithSideEffectStep : Step, ISourceStep
    {
        public WithSideEffectStep(StepLabel label, object value)
        {
            ArgumentNullException.ThrowIfNull(label);
            ArgumentNullException.ThrowIfNull(value);

            Label = label;
            Value = value;
        }

        public object Value { get; }
        public StepLabel Label { get; }
    }
}
