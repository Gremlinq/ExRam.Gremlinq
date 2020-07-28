using System;
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
                        var unwound = UnwindHasPredicateStep(newStep);

                        return traversalSteps.Length == 2
                            ? (Traversal)unwound
                            : traversalSteps.SetItem(traversalSteps.Length - 2, unwound).RemoveAt(traversalSteps.Length - 1);
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

        private static Step UnwindHasPredicateStep(HasPredicateStep step)
        {
            if (step.Predicate is {} p && ContainsNull(p))
            {
                if (p.OperatorName.Equals("or", StringComparison.OrdinalIgnoreCase))
                    return new OrStep(new Traversal[] { UnwindHasPredicateStep(new HasPredicateStep(step.Key, p.Value is P innerP ? innerP : P.Eq(p.Value))), UnwindHasPredicateStep(new HasPredicateStep(step.Key, p.Other)) });

                if (p.OperatorName.Equals("and", StringComparison.OrdinalIgnoreCase))
                    return new AndStep(new Traversal[] { UnwindHasPredicateStep(new HasPredicateStep(step.Key, p.Value is P innerP ? innerP : P.Eq(p.Value))), UnwindHasPredicateStep(new HasPredicateStep(step.Key, p.Other)) });
            }

            return step;
        }

        private static bool ContainsNull(P? p)
        {
            return p?.Value == null || p?.Other?.Value == null || (p?.Value is P firstP && ContainsNull(firstP)) || (p?.Other?.Value is P otherP && ContainsNull(otherP));
        }
    }
}
