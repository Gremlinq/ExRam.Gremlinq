﻿using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class LimitStep : Step
    {
        public static readonly LimitStep LimitLocal1 = new(1, Scope.Local);
        public static readonly LimitStep LimitGlobal1 = new(1, Scope.Global);

        internal static readonly MapStep LimitLocal1Workaround = new (Traversal.Empty.Push(
            UnfoldStep.Instance,
            LimitGlobal1,
            FoldStep.Instance));

        public LimitStep(long count, Scope scope)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        public long Count { get; }
        public Scope Scope { get; }
    }
}
