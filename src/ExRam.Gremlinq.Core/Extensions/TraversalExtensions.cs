using System.Collections.Immutable;
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

        public static Traversal Rewrite(this Traversal traversal, ContinuationFlags flags)
        {
            if (flags.HasFlag(ContinuationFlags.Filter))
            {
                if (traversal.Count == 1 && traversal[0] is FilterStep.ByTraversalStep filterStep)
                    return filterStep.Traversal.Rewrite(flags);

                if (traversal.RewriteForIsContext() is { } rewrittenTraversal)
                    return rewrittenTraversal.Rewrite(flags);
            }

            return traversal;
        }

        private static Traversal? RewriteForIsContext(this Traversal traversal, P? maybeExistingPredicate = null)
        {
            if (traversal.Count >= 1)
            {
                if (traversal[^1] is IsStep { Predicate: { } isPredicate })
                {
                    if (maybeExistingPredicate is { } existingPredicate1)
                        isPredicate = isPredicate.And(existingPredicate1);

                    return traversal
                        .Pop()
                        .RewriteForIsContext(isPredicate);
                }

                if (maybeExistingPredicate is { } existingPredicate2)
                { 
                    var newStep = traversal[^1] switch
                    {
                        IdStep => new HasPredicateStep(T.Id, existingPredicate2),
                        LabelStep => new HasPredicateStep(T.Label, existingPredicate2),
                        ValuesStep { Keys.Length: 1 } valuesStep => existingPredicate2.GetFilterStep(valuesStep.Keys[0]),
                        _ => default
                    };

                    if (newStep != null)
                    {
                        return Traversal.Create(
                            traversal.Count,
                            (traversal, newStep),
                            static (steps, state) =>
                            {
                                var (traversal, newStep) = state;

                                traversal
                                    .AsSpan()[..^1]
                                    .CopyTo(steps);

                                steps[^1] = newStep;
                            });
                    }
                }
            }
            
            return default;
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
                        var dict = new Dictionary<Key, P>();

                        for (var i = 0; i < traversals.Count; i++)
                        {
                            var step = (HasPredicateStep)traversals[i][0];

                            var key = step.Key;
                            var predicate = step.Predicate;

                            if (dict.TryGetValue(key, out var fusedP))
                                predicate = fuse(fusedP, predicate);

                            dict[key] = predicate;
                        }

                        foreach (var kvp in dict)
                        {
                            yield return new HasPredicateStep(kvp.Key, kvp.Value);
                        }

                        yield break;
                    }

                    if (isFirstIsStep)
                    {
                        var maybeFusedP = default(P?);

                        for (var i = 0; i < traversals.Count; i++)
                        {
                            var predicate = ((IsStep)traversals[i][0]).Predicate;

                            if (maybeFusedP is { } fusedP)
                                predicate = fuse(fusedP, predicate);

                            maybeFusedP = predicate;
                        }

                        yield return new IsStep(maybeFusedP!);
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

        public static bool IsNone(this Traversal traversal) => traversal.PeekOrDefault() is NoneStep;

        public static Step Peek(this Traversal traversal) => traversal.PeekOrDefault() ?? throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

        public static Step? PeekOrDefault(this Traversal traversal) => traversal.Count > 0 ? traversal[traversal.Count - 1] : null;
    }
}
