namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AddEStep : Step
    {
        public sealed class FromLabelStep : Step
        {
            public FromLabelStep(StepLabel stepLabel) : base()
            {
                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        public sealed class FromTraversalStep : Step
        {
            public FromTraversalStep(Traversal traversal) : base()
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public sealed class ToLabelStep : Step
        {
            public ToLabelStep(StepLabel stepLabel) : base()
            {
                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        public sealed class ToTraversalStep : Step
        {
            public ToTraversalStep(Traversal traversal) : base()
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public AddEStep(string label) : base(SideEffectSemanticsChange.Write)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
