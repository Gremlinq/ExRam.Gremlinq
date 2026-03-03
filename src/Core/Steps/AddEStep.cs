namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>addE()</c> step that adds an edge to the graph.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
    public sealed class AddEStep : Step
    {
        /// <summary>Represents the <c>from()</c> modulator referencing a step label as the outgoing vertex of an edge.</summary>
        public sealed class FromLabelStep : Step
        {
            /// <summary>Initializes a new instance of <see cref="FromLabelStep"/>.</summary>
            /// <param name="stepLabel">The step label referencing the outgoing vertex.</param>
            public FromLabelStep(StepLabel stepLabel)
            {
                ArgumentNullException.ThrowIfNull(stepLabel);

                StepLabel = stepLabel;
            }

            /// <summary>Gets the step label referencing the outgoing vertex.</summary>
            public StepLabel StepLabel { get; }
        }

        /// <summary>Represents the <c>from()</c> modulator referencing a traversal as the outgoing vertex of an edge.</summary>
        public sealed class FromTraversalStep : Step
        {
            /// <summary>Initializes a new instance of <see cref="FromTraversalStep"/>.</summary>
            /// <param name="traversal">The traversal selecting the outgoing vertex.</param>
            public FromTraversalStep(Traversal traversal)
            {
                Traversal = traversal;
            }

            /// <summary>Gets the traversal selecting the outgoing vertex.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Represents the <c>to()</c> modulator referencing a step label as the incoming vertex of an edge.</summary>
        public sealed class ToLabelStep : Step
        {
            /// <summary>Initializes a new instance of <see cref="ToLabelStep"/>.</summary>
            /// <param name="stepLabel">The step label referencing the incoming vertex.</param>
            public ToLabelStep(StepLabel stepLabel)
            {
                ArgumentNullException.ThrowIfNull(stepLabel);

                StepLabel = stepLabel;
            }

            /// <summary>Gets the step label referencing the incoming vertex.</summary>
            public StepLabel StepLabel { get; }
        }

        /// <summary>Represents the <c>to()</c> modulator referencing a traversal as the incoming vertex of an edge.</summary>
        public sealed class ToTraversalStep : Step
        {
            /// <summary>Initializes a new instance of <see cref="ToTraversalStep"/>.</summary>
            /// <param name="traversal">The traversal selecting the incoming vertex.</param>
            public ToTraversalStep(Traversal traversal)
            {
                Traversal = traversal;
            }

            /// <summary>Gets the traversal selecting the incoming vertex.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Initializes a new instance of <see cref="AddEStep"/> with the specified edge label.</summary>
        /// <param name="label">The label of the edge to add.</param>
        public AddEStep(string label) : base(SideEffectSemanticsChange.Write)
        {
            ArgumentNullException.ThrowIfNull(label);

            Label = label;
        }

        /// <summary>Gets the label of the edge to add.</summary>
        public string Label { get; }
    }
}
