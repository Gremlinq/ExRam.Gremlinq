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

                        return new Traversal(list, traversal.Projection);
                    }
                }
            }
            else if (traversal.Count == 1)
            {
                if (traversal[0] is FilterStep.ByTraversalStep filterStep)
                    return filterStep.Traversal.RewriteForWhereContext();
            }

            return traversal;
        }

        public static IEnumerable<Traversal> Fuse(
            this ArraySegment<Traversal> traversals,
            Func<P, P, P> fuse)
        {
            if (traversals.Count > 0)
            {
                var isCount1 = true;
                var isFirstHasPredicateStep = true;
                var isFirstIsStep = true;

                for (var i = 0; i < traversals.Count; i++)
                {
                    if (traversals.Array![i].Count == 1)
                    {
                        if (traversals.Array[i][0] is not HasPredicateStep)
                            isFirstHasPredicateStep = false;

                        if (traversals.Array[i][0] is not IsStep)
                            isFirstIsStep = false;
                    }
                    else
                        isCount1 = false;
                }

                if (isCount1)
                {
                    if (isFirstHasPredicateStep)
                    {
                        var groups = traversals
                            .GroupBy(
                                static x => ((HasPredicateStep)x[0]).Key,
                                static x => ((HasPredicateStep)x[0]).Predicate);

                        foreach (var group in groups)
                        {
                            var effective = group
                                .Aggregate(fuse);

                            yield return new HasPredicateStep(group.Key, effective);
                        }

                        yield break;
                    }

                    if (isFirstIsStep)
                    {
                        var effective = traversals
                            .Select(static x => ((IsStep)x[0]).Predicate)
                            .Aggregate(fuse);

                        yield return new IsStep(effective);
                        yield break;
                    }
                }

                for (var i = 0; i < traversals.Count; i++)
                {
                    yield return traversals.Array![i];
                }
            }
        }

        public static bool IsIdentity(this Traversal traversal) => traversal.Count == 0 || (traversal.Count == 1 && traversal[0] is IdentityStep);

        public static bool IsNone(this Traversal traversal) => traversal.Count > 0 && traversal[traversal.Count - 1] is NoneStep;
    }
}
