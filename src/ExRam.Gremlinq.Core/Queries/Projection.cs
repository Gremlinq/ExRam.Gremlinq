using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Projection : IReadOnlyList<Step>
    {
        public static readonly Projection Empty = new(ImmutableArray<Step>.Empty);

        private readonly IReadOnlyList<Step> _steps;

        public Projection(IEnumerable<Step> steps) : this(steps.ToArray(), true)
        {
        }

        public Projection(ImmutableArray<Step> steps)
        {
            _steps = steps;
        }

        public Projection Decorate(Projection? before, Projection? after)
        {
            if (Count > 0 && (before != null || after != null))
            {
                var proj = (IEnumerable<Step>)this;

                if (before.HasValue)
                    proj = before.Value.Concat(proj);

                if (after.HasValue)
                    proj = proj.Concat(after.Value);

                return new Projection(proj);
            }

            return this;
        }

        internal Projection(IReadOnlyList<Step> steps, bool owned)
        {
            _steps = owned
                ? steps
                : steps.ToArray();
        }

        public IEnumerator<Step> GetEnumerator() => _steps.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get => _steps.Count; }

        public Step this[int index] => _steps[index];

        public static implicit operator Projection(Step[] steps) => new(steps, false);

        public static implicit operator Projection(ImmutableArray<Step> steps) => new(steps);

        public static implicit operator Projection(Step step) => new(new[] { step }, true);
    }
}
