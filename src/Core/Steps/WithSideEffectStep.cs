namespace ExRam.Gremlinq.Core.Steps
{
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
