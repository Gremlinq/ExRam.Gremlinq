using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class TraversalExtensions
    {
        public static SideEffectSemanticsChange GetSideEffectSemanticsChange(this IEnumerable<Traversal> traversals)
        {
            foreach (var traversal in traversals)
            {
                if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                    return SideEffectSemanticsChange.Write;
            }

            return SideEffectSemanticsChange.None;
        }

        public static SideEffectSemanticsChange GetSideEffectSemanticsChange(this ImmutableArray<Traversal> traversals)
        {
            for (var i = 0;  i < traversals.Length; i++)
            {
                if (traversals[i].SideEffectSemantics == SideEffectSemantics.Write)
                    return SideEffectSemanticsChange.Write;
            }

            return SideEffectSemanticsChange.None;
        }

        public static SideEffectSemanticsChange GetSideEffectSemanticsChange(this Traversal traversal)
        {
            return (SideEffectSemanticsChange)traversal.SideEffectSemantics;
        }

        public static SideEffectSemanticsChange GetSideEffectSemanticsChange(this Traversal? maybeTraversal)
        {
            return maybeTraversal is { } traversal
                ? traversal.GetSideEffectSemanticsChange()
                : SideEffectSemanticsChange.None;
        }

        public static Traversal RewriteForWhereContext(this Traversal traversal)
        {
            if (traversal.Count >= 2)
            {
                if (traversal[traversal.Count - 1] is IsStep isStep)
                {
                    var newStep = traversal[traversal.Count - 2] switch
                    {
                        ValuesStep { Keys.Length: 1 } valuesStep => new HasPredicateStep(valuesStep.Keys[0], isStep.Predicate),
                        IdStep => new HasPredicateStep(T.Id, isStep.Predicate),
                        LabelStep => new HasPredicateStep(T.Label, isStep.Predicate),
                        _ => default
                    };

                    if (newStep != null)
                    {
                        if (traversal.Count == 2)
                            return newStep;

                        var list = traversal.ToList();
                        list[traversal.Count - 2] = newStep;
                        list.RemoveAt(traversal.Count - 1);

                        return new Traversal(list, true, traversal.Projection);
                    }
                }
            }
            else if (traversal.Count == 1)
            {
                if (traversal[0] is WhereTraversalStep whereTraversalStep)
                    return whereTraversalStep.Traversal.RewriteForWhereContext();
            }

            return traversal;
        }

        public static IEnumerable<Traversal> Fuse(
            this IEnumerable<Traversal> traversals,
            Func<P, P, P> fuse)
        {
            var traversalsArray = traversals.ToArray();

            if (traversalsArray.Any())
            {
                if (traversalsArray.All(x => x.Count == 1))
                {
                    if (traversalsArray.All(x => x[0] is HasPredicateStep))
                    {
                        var groups = traversalsArray
                            .Select(x => x[0])
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

                    if (traversalsArray.All(x => x[0] is IsStep))
                    {
                        var effective = traversalsArray
                            .Select(x => x[0])
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

        public static bool IsIdentity(this Traversal traversal) => traversal.Count == 0;

        public static bool IsNone(this Traversal traversal) => traversal.Count > 0 && traversal[traversal.Count - 1] is NoneStep;
    }
}
