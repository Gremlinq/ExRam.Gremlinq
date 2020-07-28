using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class TraversalExtensions
    {
        public static Traversal RewriteForWhereContext(this Traversal traversal)
        {
            var traversalSteps = traversal.Steps;

            if (traversalSteps.Length >= 2)
            {
                if (traversalSteps[traversalSteps.Length - 1] is IsStep isStep)
                {
                    var newStep = traversalSteps[traversalSteps.Length - 2] switch
                    {
                        ValuesStep valuesStep when valuesStep.Keys.Length == 1 => new HasPredicateStep(valuesStep.Keys[0], isStep.Predicate),
                        IdStep _ => new HasPredicateStep(T.Id, isStep.Predicate),
                        LabelStep _ => new HasPredicateStep(T.Label, isStep.Predicate),
                        _ => default
                    };

                    if (newStep != null)
                    {
                        return traversalSteps.Length == 2
                            ? (Traversal)newStep
                            : traversalSteps.SetItem(traversalSteps.Length - 2, newStep).RemoveAt(traversalSteps.Length - 1);
                    }
                }
            }
            else if (traversalSteps.Length == 1)
            {
                if (traversalSteps[0] is HasPredicateStep hasPredicateStep)
                    return hasPredicateStep;

                if (traversalSteps[0] is WhereTraversalStep whereTraversalStep)
                    return whereTraversalStep.Traversal.RewriteForWhereContext();
            }

            return traversal;
        }
    }
}