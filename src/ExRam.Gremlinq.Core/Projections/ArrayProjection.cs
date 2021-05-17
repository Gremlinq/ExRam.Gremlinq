using System.Collections.Immutable;
using System.Linq;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class ArrayProjection : Projection
    {
        private readonly Projection _inner;

        internal ArrayProjection(Projection inner)
        {
            _inner = inner;
        }

        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            var inner = _inner.ToTraversal(environment);

            if (inner.Count > 0)
            {
                return new LocalStep(inner
                    .Prepend(UnfoldStep.Instance)
                    .Append(FoldStep.Instance)
                    .ToImmutableArray());
            }

            return Traversal.Empty;
        }

        public Projection Unfold() => _inner;

        public override Projection BaseProjection => None;
    }
}
