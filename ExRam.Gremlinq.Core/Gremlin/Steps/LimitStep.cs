using System;
using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class LimitStep : Step
    {
        public static readonly LimitStep LimitLocal1 = new LimitStep(1, Scope.Local);
        public static readonly LimitStep Limit1 = new LimitStep(1, Scope.Global);

        public LimitStep(long count, Scope scope)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
            Scope = scope;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public long Count { get; }
        public Scope Scope { get; }
    }
}
