namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>addE()</c> step that adds an edge to the graph.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
    public sealed class AddEStep : Step
    {
        /// <summary>Represents the <c>from()</c> modulator referencing a step label as the outgoing vertex of an edge.</summary>
        public sealed class FromLabelStep : Step
        {
            public FromLabelStep(StepLabel stepLabel)
            {
                ArgumentNullException.ThrowIfNull(stepLabel);

                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        /// <summary>Represents the <c>from()</c> modulator referencing a traversal as the outgoing vertex of an edge.</summary>
        public sealed class FromTraversalStep : Step
        {
            public FromTraversalStep(Traversal traversal)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        /// <summary>Represents the <c>to()</c> modulator referencing a step label as the incoming vertex of an edge.</summary>
        public sealed class ToLabelStep : Step
        {
            public ToLabelStep(StepLabel stepLabel)
            {
                ArgumentNullException.ThrowIfNull(stepLabel);

                StepLabel = stepLabel;
            }

            public StepLabel StepLabel { get; }
        }

        /// <summary>Represents the <c>to()</c> modulator referencing a traversal as the incoming vertex of an edge.</summary>
        public sealed class ToTraversalStep : Step
        {
            public ToTraversalStep(Traversal traversal)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public AddEStep(string label) : base(SideEffectSemanticsChange.Write)
        {
            ArgumentNullException.ThrowIfNull(label);

            Label = label;
        }

        public string Label { get; }
    }
}
