using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class AggregateStep : Step
    {
        public AggregateStep(Scope scope, StepLabel stepLabel) : base()
        {
            Scope = scope;
            StepLabel = stepLabel;
        }

        public Scope Scope { get; }
        public StepLabel StepLabel { get; }
    }
}
