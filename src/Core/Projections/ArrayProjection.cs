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
                return new MapStep(Traversal.Create(
                    inner.Count + 2,
                    inner,
                    static (steps, inner) =>
                    {
                        steps[0] = UnfoldStep.Instance;
                        steps[^1] = FoldStep.Instance;

                        inner
                            .Steps
                            .CopyTo(steps[1..]);
                    }));
            }

            return Traversal.Empty;
        }

        public Projection Unfold() => _inner;

        public override Projection Lower() => Empty;
    }
}
