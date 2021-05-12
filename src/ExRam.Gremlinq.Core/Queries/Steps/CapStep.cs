namespace ExRam.Gremlinq.Core
{
    public sealed class CapStep : Step
    {
        public CapStep(StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
        {
            StepLabel = stepLabel;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new CapStep(StepLabel, semantics);

        public StepLabel StepLabel { get; }
    }
}
