using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class AggregateStep : Step
    {
        public AggregateStep(Scope scope, StepLabel stepLabel) : base(SideEffectSemanticsChange.Write)
        {
            Scope = scope;
            StepLabel = stepLabel;
        }

        public Scope Scope { get; }
        public StepLabel StepLabel { get; }
    }
}
