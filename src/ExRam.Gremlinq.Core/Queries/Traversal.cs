using System.Collections.Generic;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal
    {
        public Traversal(IEnumerable<Step> steps) : this(steps.ToImmutableArray())
        {
        }

        public Traversal(ImmutableArray<Step> steps)
        {
            Steps = steps;
        }

        public ImmutableArray<Step> Steps { get; }

        public static implicit operator Traversal(Step[] steps)
        {
            return new(steps);
        }

        public static implicit operator Traversal(ImmutableArray<Step> steps)
        {
            return new(steps);
        }

        public static implicit operator Traversal(Step step)
        {
            return new(ImmutableArray.Create(step));
        }
    }
}
