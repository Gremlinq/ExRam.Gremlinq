using System;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public sealed class TailStep : Step
    {
        public static readonly TailStep TailLocal1 = new(1, Scope.Local);
        public static readonly TailStep TailGlobal1 = new(1, Scope.Global);

        public TailStep(long count, Scope scope, QuerySemantics? semantics = default) : base(semantics)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new TailStep(Count, Scope, semantics);

        public long Count { get; }
        public Scope Scope { get; }
    }
}
