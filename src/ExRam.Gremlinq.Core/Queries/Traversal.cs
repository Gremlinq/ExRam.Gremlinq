using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        public static readonly Traversal Empty = new(ImmutableArray<Step>.Empty, true, Projection.None);

        private readonly IReadOnlyList<Step> _steps;

        public Traversal(IEnumerable<Step> steps, Projection projection) : this(steps.ToArray(), true, projection)
        {
        }

        public Traversal(ImmutableArray<Step> steps, Projection projection) : this(steps, true, projection)
        {
        }

        internal Traversal(IReadOnlyList<Step> steps, bool owned, Projection projection)
        {
            _steps = owned
                ? steps
                : steps.ToArray();

            Projection = projection;
        }

        public IEnumerator<Step> GetEnumerator() => _steps.GetEnumerator();

        public Traversal IncludeProjection(IGremlinQueryEnvironment environment)
        {
            if (Projection == Projection.None)
                return this;

            return new Traversal(this.Concat(Projection.Expand(environment)), Projection.None);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get => _steps.Count; }

        public Projection Projection { get; }

        public Step this[int index] => _steps[index];

        public static implicit operator Traversal(Step[] steps) => new(steps, false, Projection.None);

        public static implicit operator Traversal(ImmutableArray<Step> steps) => new(steps, Projection.None);

        public static implicit operator Traversal(Step step) => new(new[] { step }, true, Projection.None);
    }
}
