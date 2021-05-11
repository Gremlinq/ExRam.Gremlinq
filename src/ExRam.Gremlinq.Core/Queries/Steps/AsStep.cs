namespace ExRam.Gremlinq.Core
{
    public sealed class AsStep : Step
    {
        public AsStep(StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
        {
            StepLabel = stepLabel;
        }

        public StepLabel StepLabel { get; }
    }
}
