using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class WhereStepLabelAndPredicateStep : Step, IIsOptimizableInWhere
    {
        public WhereStepLabelAndPredicateStep(StepLabel stepLabel, P predicate) : base()
        {
            StepLabel = stepLabel;
            Predicate = predicate;
        }

        public P Predicate { get; }
        public StepLabel StepLabel { get; }
    }
}
