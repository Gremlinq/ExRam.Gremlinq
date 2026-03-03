using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>where(label, predicate)</c> step that filters by comparing a step label value to a predicate.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#where-step">Reference Documentation - Where Step</seealso>
    public sealed class WhereStepLabelAndPredicateStep : Step, IFilterStep
    {
        /// <summary>Initializes a new instance of <see cref="WhereStepLabelAndPredicateStep"/>.</summary>
        /// <param name="stepLabel">The step label whose value is compared.</param>
        /// <param name="predicate">The predicate to apply.</param>
        public WhereStepLabelAndPredicateStep(StepLabel stepLabel, P predicate)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);
            ArgumentNullException.ThrowIfNull(predicate);

            StepLabel = stepLabel;
            Predicate = predicate;
        }

        /// <summary>Gets the predicate.</summary>
        public P Predicate { get; }
        /// <summary>Gets the step label whose value is compared.</summary>
        public StepLabel StepLabel { get; }
    }
}
