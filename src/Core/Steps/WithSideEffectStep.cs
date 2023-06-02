namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class WithSideEffectStep : Step, ISourceStep
    {
        public WithSideEffectStep(StepLabel label, object value)
        {
            Label = label;
            Value = value;
        }

        public object Value { get; }
        public StepLabel Label { get; }
    }
}
