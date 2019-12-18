namespace ExRam.Gremlinq.Core
{
    public sealed class AddEStep : AddElementStep
    {
        public sealed class ToLabelStep : Step
        {
            public ToLabelStep(StepLabel stepLabel)
            {
                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        public sealed class ToTraversalStep : SingleTraversalArgumentStep
        {
            public ToTraversalStep(IGremlinQueryBase traversal) : base(traversal)
            {
            }
        }

        public AddEStep(IGraphModel model, object value) : base(model.EdgesModel, value)
        {
        }
    }
}
