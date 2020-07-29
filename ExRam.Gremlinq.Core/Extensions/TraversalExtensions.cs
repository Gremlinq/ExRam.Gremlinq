using System;
using System.Collections.Generic;
using System.Linq;
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
                if (traversalSteps[0] is WhereTraversalStep whereTraversalStep)
                    return whereTraversalStep.Traversal.RewriteForWhereContext();
            }

            return traversal;
        }

        public static IEnumerable<Traversal> Fuse(
            this IEnumerable<Traversal> traversals,
            Func<P?, P?, P?> fuse)
        {
            var traversalsArray = traversals.ToArray();

            if (traversalsArray.Any())
            {
                if (traversalsArray.All(x => x.Steps.Length == 1))
                {
                    if (traversalsArray.All(x => x.Steps[0] is HasPredicateStep))
                    {
                        var groups = traversalsArray
                            .Select(x => x.Steps[0])
                            .OfType<HasPredicateStep>()
                            .GroupBy(x => x.Key);

                        foreach (var group in groups)
                        {
                            var effective = group
                                .Select(x => x.Predicate)
                                .Aggregate(fuse);

                            yield return new HasPredicateStep(group.Key, effective);
                        }

                        yield break;
                    }

                    if (traversalsArray.All(x => x.Steps[0] is IsStep))
                    {
                        var effective = traversalsArray
                            .Select(x => x.Steps[0])
                            .OfType<IsStep>()
                            .Select(x => x.Predicate)
                            .Aggregate(fuse);

                        if (effective != null)
                        {
                            yield return new IsStep(effective);
                            yield break;
                        }
                    }
                }

                foreach (var traversal in traversalsArray)
                {
                    yield return traversal;
                }
            }
        }
    }
}
