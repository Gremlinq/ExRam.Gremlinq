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

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new FromLabelStep(StepLabel, semantics);

            public StepLabel StepLabel { get; }
        }

        public sealed class FromTraversalStep : Step
        {
            public FromTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
            }

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new FromTraversalStep(Traversal, semantics);

            public Traversal Traversal { get; }
        }

        public sealed class ToLabelStep : Step
        {
            public ToLabelStep(StepLabel stepLabel, QuerySemantics? semantics = default) : base(semantics)
            {
                StepLabel = stepLabel;
            }

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ToLabelStep(StepLabel, semantics);

            public StepLabel StepLabel { get; }
        }

        public sealed class ToTraversalStep : Step
        {
            public ToTraversalStep(Traversal traversal, QuerySemantics? semantics = default) : base(semantics)
            {
                Traversal = traversal;
            }

            public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ToTraversalStep(Traversal, semantics);

            public Traversal Traversal { get; }
        }

        public AddEStep(string label, QuerySemantics? semantics = default) : base(semantics)
        {
            Label = label;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new AddEStep(Label, semantics);

        public string Label { get; }
    }
}
