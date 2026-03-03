namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>withSideEffect()</c> source step that registers a side-effect on the traversal source.</summary>
    public sealed class WithSideEffectStep : Step, ISourceStep
    {
        /// <summary>Initializes a new instance of <see cref="WithSideEffectStep"/>.</summary>
        /// <param name="label">The step label identifying the side-effect.</param>
        /// <param name="value">The value to register as a side-effect.</param>
        public WithSideEffectStep(StepLabel label, object value)
        {
            ArgumentNullException.ThrowIfNull(label);
            ArgumentNullException.ThrowIfNull(value);

            Label = label;
            Value = value;
        }

        /// <summary>Gets the side-effect value.</summary>
        public object Value { get; }
        /// <summary>Gets the step label identifying the side-effect.</summary>
        public StepLabel Label { get; }
    }
}
