using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        private readonly IReadOnlyList<Step> _steps;

        public Traversal(IEnumerable<Step> steps) : this(steps.ToArray(), true)
        {
        }

        public Traversal(ImmutableArray<Step> steps)
        {
            _steps = steps;
        }

        internal Traversal(IReadOnlyList<Step> steps, bool owned)
        {
            _steps = owned
                ? steps
                : steps.ToArray();
        }

        public IEnumerator<Step> GetEnumerator() => _steps.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get => _steps.Count; }

        public Step this[int index] => _steps[index];

        public static implicit operator Traversal(Step[] steps) => new(steps, false);

        public static implicit operator Traversal(ImmutableArray<Step> steps) => new(steps);

        public static implicit operator Traversal(Step step) => new(new[] { step }, true);
    }
}
