namespace ExRam.Gremlinq.Core
{
    public sealed class WithSideEffectStep : Step
    {
        public WithSideEffectStep(StepLabel label, object value, QuerySemantics? semantics = default) : base(semantics)
        {
            Label = label;
            Value = value;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new WithSideEffectStep(Label, Value, semantics);

        public object Value { get; }
        public StepLabel Label { get; }
    }
}
