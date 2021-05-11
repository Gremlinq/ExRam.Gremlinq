namespace ExRam.Gremlinq.Core
{
    public sealed class AddEStep : Step
    {
        public sealed class FromLabelStep : Step
        {
            public FromLabelStep(StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
            {
                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        public sealed class FromTraversalStep : Step
        {
            public FromTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public sealed class ToLabelStep : Step
        {
            public ToLabelStep(StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
            {
                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        public sealed class ToTraversalStep : Step
        {
            public ToTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public AddEStep(string label, QuerySemantics? semantics = default) : base(semantics)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
