﻿using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class LogicalStep<TStep> : Step
        where TStep : LogicalStep<TStep>
    {
        protected LogicalStep(string name, IEnumerable<Traversal> traversals) : this(
            name,
            traversals
                .SelectMany(FlattenLogicalTraversals)
                .ToImmutableArray())
        {
        }

        private LogicalStep(string name, ImmutableArray<Traversal> traversals) : base(traversals.GetSideEffectSemanticsChange())
        {
            Name = name;
            Traversals = traversals;
        }

        private static IEnumerable<Traversal> FlattenLogicalTraversals(Traversal traversal)
        {
            if (traversal is [TStep otherStep])
            {
                foreach (var subTraversal in otherStep.Traversals)
                {
                    foreach (var flattenedSubTraversal in FlattenLogicalTraversals(subTraversal))
                    {
                        yield return flattenedSubTraversal;
                    }
                }
            }
            else
                yield return traversal;
        }

        //TODO: Seemingly unused.
        public string Name { get; }
        public ImmutableArray<Traversal> Traversals { get; }
    }
}
