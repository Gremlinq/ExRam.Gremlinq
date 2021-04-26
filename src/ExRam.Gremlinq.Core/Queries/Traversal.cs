using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

        internal Traversal(IReadOnlyList<Step> steps, bool owned)
        {
            Steps = owned
                ? steps
                : steps.ToArray();
        }

        public IReadOnlyList<Step> Steps { get; }

        public static implicit operator Traversal(Step[] steps)
        {
            return new(steps, false);
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
