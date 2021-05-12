using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class LimitStep : Step
    {
        public static readonly LimitStep LimitLocal1 = new(1, Scope.Local);
        public static readonly LimitStep LimitGlobal1 = new(1, Scope.Global);

        public LimitStep(long count, Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new LimitStep(Count, Scope, semantics);

        public long Count { get; }
        public Scope Scope { get; }
    }
}
