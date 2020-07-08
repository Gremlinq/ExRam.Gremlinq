using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class TraversalExtensions
    {
        public static Traversal RewriteForWhereContext(this Traversal traversal)
        {
            var traversalSteps = traversal.Steps;

            if (traversalSteps.Length == 2)
            {
                if (traversalSteps[1] is IsStep isStep)
                {
                    if (traversalSteps[0] is ValuesStep valuesStep && valuesStep.Keys.Length == 1)
                        return new HasPredicateStep(valuesStep.Keys[0], isStep.Predicate);

                    if (traversalSteps[0] is IdStep)
                        return new HasPredicateStep(T.Id, isStep.Predicate);

                    if (traversalSteps[0] is LabelStep)
                        return new HasPredicateStep(T.Label, isStep.Predicate);
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

    internal static class StringExtensions
    {
        public static string ToCamelCase(this string source)
        {
            if (source.Length < 2)
                return source;

            return source.Substring(0, 1).ToLower() + source.Substring(1);
        }
    }
}
