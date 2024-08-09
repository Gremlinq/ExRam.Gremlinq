using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
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
            if (traversal is [NoneStep _, ..])
            {
                return Traversal
                    .Create(
                        traversal.Count + 1,
                        traversal,
                        static (steps, traversal) =>
                        {
                            steps[0] = IdentityStep.Instance;
                            traversal.Steps.CopyTo(steps[1..]);
                        })
                    .Rewrite(flags);
            }

            if ((flags & ContinuationFlags.Filter) == ContinuationFlags.Filter)
            {
                if (traversal is [FilterStep.ByTraversalStep filterStep])
                    return filterStep.Traversal.Rewrite(flags);

                if (traversal.RewriteForIsContext() is { } rewrittenTraversal)
                    return rewrittenTraversal.Rewrite(flags);
            }

            return traversal;
        }

        private static Traversal? RewriteForIsContext(this Traversal traversal, P? maybeExistingPredicate = null)
        {
            if (traversal is [.., var lastStep])
            {
                if (lastStep is IsStep { Predicate: { } isPredicate })
                {
                    if (maybeExistingPredicate is { } existingPredicate1)
                        isPredicate = isPredicate.And(existingPredicate1);

                    return traversal
                        .Pop()
                        .RewriteForIsContext(isPredicate);
                }

                if (maybeExistingPredicate is { } existingPredicate2)
                {
                    var newStep = lastStep switch
                    {
                        IdStep => new HasPredicateStep(T.Id, existingPredicate2),
                        LabelStep => new HasPredicateStep(T.Label, existingPredicate2),
                        ValuesStep { Keys.Length: 1 } valuesStep => existingPredicate2.GetFilterStep(valuesStep.Keys[0]),
                        _ => default
                    };

                    if (newStep != null)
                    {
                        return traversal
                            .Pop()
                            .Push(newStep);
                    }
                }
            }
            
            return default;
        }

        public static Projection LowestProjection(this Span<Traversal> traversals)
        {
            if (traversals is [var first, .. var remainder])
            {
                var projection = first.Projection;

                foreach (var traversal in remainder)
                {
                    projection = projection.Lowest(traversal.Projection);
                }

                return projection;
            }

            return Projection.Empty;
        }

        public static Span<Traversal> Fuse(
            this Span<Traversal> traversals,
            Func<P, P, P> fuse)
        {
            if (traversals.Length > 0)
            {
                var isCount1 = true;
                var isFirstIsStep = true;
                var isFirstHasPredicateStep = true;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.Count == 1)
                    {
                        if (traversal[0] is not HasPredicateStep)
                            isFirstHasPredicateStep = false;

                        if (traversal[0] is not IsStep)
                            isFirstIsStep = false;
                    }
                    else
                        isCount1 = false;
                }

                if (isCount1)
                {
                    if (isFirstHasPredicateStep)
                    {
                        var count = 0;
                        var dict = new Dictionary<Key, P>();

                        for (var i = 0; i < traversals.Length; i++)
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
                            traversals[count++] = new HasPredicateStep(kvp.Key, kvp.Value);
                        }

                        return traversals[..count];
                    }

                    if (isFirstIsStep)
                    {
                        var maybeFusedP = default(P?);

                        for (var i = 0; i < traversals.Length; i++)
                        {
                            var predicate = ((IsStep)traversals[i][0]).Predicate;

                            if (maybeFusedP is { } fusedP)
                                predicate = fuse(fusedP, predicate);

                            maybeFusedP = predicate;
                        }

                        traversals[0] = new IsStep(maybeFusedP!);

                        return traversals[..1];
                    }
                }
            }

            return traversals;
        }

        public static bool IsIdentity(this Traversal traversal) => traversal.Count == 0 || (traversal is [IdentityStep, .. var remainder] && remainder.IsIdentity());

        public static bool IsNone(this Traversal traversal) => traversal.PeekOrDefault() is NoneStep;

        public static Step Peek(this Traversal traversal) => traversal.PeekOrDefault() ?? throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

        public static Step? PeekOrDefault(this Traversal traversal) => traversal.Count > 0 ? traversal[^1] : null;
    }
}
