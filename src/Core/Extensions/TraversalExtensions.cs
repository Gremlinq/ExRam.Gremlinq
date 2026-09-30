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

        public static SideEffectSemanticsChange GetSideEffectSemanticsChange(this Traversal traversal) => (SideEffectSemanticsChange)traversal.SideEffectSemantics;

        public static Traversal Rewrite(this Traversal traversal, ContinuationFlags flags)
        {
            if (traversal is [NoneStep, ..])
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
                        _ => null
                    };

                    if (newStep != null)
                    {
                        return traversal
                            .Pop()
                            .Push(newStep);
                    }
                }
            }
            
            return null;
        }

        public static Projection? LowestProjection(this Span<Traversal> traversals)
        {
            if (traversals is [var first, .. var remainder])
            {
                if (first.IsNone())
                    return remainder.LowestProjection();

                if (remainder.LowestProjection() is { } lowestRemainder)
                    return first.Projection.Lowest(lowestRemainder);

                return first.Projection;
            }

            return null;
        }

        public static Span<Traversal> Fuse(this Span<Traversal> traversals, Func<P, P, P> fuse)
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

        public static bool IsIdentity(this Traversal traversal) => traversal is [] || (traversal is [IdentityStep, .. var remainder] && remainder.IsIdentity());

        public static bool IsNone(this Traversal traversal) => traversal.PeekOrDefault() is NoneStep;

        // True if every step of the traversal behaves the same whether the traversal is evaluated
        // once per traverser, as a local child of e.g. coalesce(), or inlined into the parent's stream.
        public static bool IsTraverserLocal(this Traversal traversal)
        {
            if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                return false;

            var steps = traversal.Steps;

            for (var i = 0; i < steps.Length; i++)
            {
                if (!steps[i].IsTraverserLocal(i == 0))
                    return false;
            }

            return true;
        }

        // A conservative allow-list: a step that is not listed here is not traverser-local.
        private static bool IsTraverserLocal(this Step step, bool isFirst) => step switch
        {
            // Filters decide on one traverser at a time.
            IdentityStep or IFilterStep or FilterStep.ByTraversalStep => true,

            // Navigation and flat maps start from one traverser at a time.
            OutStep or InStep or BothStep or OutEStep or InEStep or BothEStep => true,
            OutVStep or InVStep or BothVStep or OtherVStep => true,
            PropertiesStep or ValuesStep or UnfoldStep => true,

            // Maps turn one traverser into one other.
            IdStep or LabelStep or KeyStep or ValueStep or ValueMapStep or ElementMapStep or ConstantStep => true,
            SelectColumnStep or SelectKeysStep => true,

            // String and date functions work on one traverser, whatever their scope.
            AsStringStep or AsDateStep or DateAddStep or DateDiffStep => true,
            ConcatStringsStep or ConcatTraversalsStep or FormatStep or LengthStep or ReplaceStep or ReverseStep or SubstringStep => true,
            ToLowerStep or ToUpperStep or TrimStep or TrimStartStep or TrimEndStep => true,

            // The traversals of these steps are local children, whatever is inside them.
            MapStep or FlatMapStep or LocalStep or CoalesceStep or ProjectStep => true,

            // Locally scoped, these steps work on the collection within one traverser. Globally scoped, they work on the stream.
            LimitStep { Scope: var scope } => Scope.Local.Equals(scope),
            RangeStep { Scope: var scope } => Scope.Local.Equals(scope),
            TailStep { Scope: var scope } => Scope.Local.Equals(scope),
            SkipStep { Scope: var scope } => Scope.Local.Equals(scope),
            DedupStep { Scope: var scope } => Scope.Local.Equals(scope),
            CountStep { Scope: var scope } => Scope.Local.Equals(scope),
            OrderStep { Scope: var scope } => Scope.Local.Equals(scope),
            MinStep { Scope: var scope } => Scope.Local.Equals(scope),
            MaxStep { Scope: var scope } => Scope.Local.Equals(scope),
            MeanStep { Scope: var scope } => Scope.Local.Equals(scope),
            SumStep { Scope: var scope } => Scope.Local.Equals(scope),

            // A modulator belongs to a step before it, which must be part of the same traversal.
            ProjectStep.ByStep or FormatStep.By or WherePredicateStep.ByMemberStep or OrderStep.ByStep => !isFirst,

            _ => false
        };

        public static Step Peek(this Traversal traversal) => traversal.PeekOrDefault() ?? throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

        public static Step? PeekOrDefault(this Traversal traversal) => traversal is [.., { } lastStep] ? lastStep : null;
    }
}
