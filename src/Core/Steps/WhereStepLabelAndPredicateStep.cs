using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>where(label, predicate)</c> step that filters by comparing a step label value to a predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
    public sealed class WhereStepLabelAndPredicateStep : Step, IFilterStep
    {
        public WhereStepLabelAndPredicateStep(StepLabel stepLabel, P predicate)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);
            ArgumentNullException.ThrowIfNull(predicate);

            StepLabel = stepLabel;
            Predicate = predicate;
        }

        public P Predicate { get; }
        public StepLabel StepLabel { get; }
    }
}
