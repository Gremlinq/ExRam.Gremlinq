using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class FromLabelStep : Step
    {
        public FromLabelStep(StepLabel stepLabel)
        {
            StepLabel = stepLabel;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public StepLabel StepLabel { get; }
    }
}
