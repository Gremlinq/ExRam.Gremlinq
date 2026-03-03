using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>aggregate()</c> step that collects traversers into a side-effect list.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#aggregate-step">Reference Documentation - Aggregate Step</seealso>
    public sealed class AggregateStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="AggregateStep"/>.</summary>
        /// <param name="scope">The scope of the aggregation (global or local).</param>
        /// <param name="stepLabel">The step label identifying the side-effect collection.</param>
        public AggregateStep(Scope scope, StepLabel stepLabel) : base(SideEffectSemanticsChange.Write)
        {
            ArgumentNullException.ThrowIfNull(scope);
            ArgumentNullException.ThrowIfNull(stepLabel);

            Scope = scope;
            StepLabel = stepLabel;
        }

        /// <summary>Gets the scope of the aggregation.</summary>
        public Scope Scope { get; }
        /// <summary>Gets the step label identifying the side-effect collection.</summary>
        public StepLabel StepLabel { get; }
    }
}
