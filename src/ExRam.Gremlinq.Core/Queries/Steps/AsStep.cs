namespace ExRam.Gremlinq.Core
{
    public sealed class AsStep : Step
    {
        public AsStep(StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
        {
            StepLabel = stepLabel;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new AsStep(StepLabel, semantics);

        public StepLabel StepLabel { get; }
    }
}
