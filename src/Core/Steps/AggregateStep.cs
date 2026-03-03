using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>aggregate()</c> step that collects traversers into a side-effect list.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#aggregate-step">Reference Documentation - Aggregate Step</seealso>
    public sealed class AggregateStep : Step
    {
        public AggregateStep(Scope scope, StepLabel stepLabel) : base(SideEffectSemanticsChange.Write)
        {
            ArgumentNullException.ThrowIfNull(scope);
            ArgumentNullException.ThrowIfNull(stepLabel);

            Scope = scope;
            StepLabel = stepLabel;
        }

        public Scope Scope { get; }
        public StepLabel StepLabel { get; }
    }
}
