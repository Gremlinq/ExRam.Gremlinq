using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for array/folded results.</summary>
    public sealed class ArrayProjection : Projection
    {
        private readonly Projection _inner;

        internal ArrayProjection(Projection inner)
        {
            _inner = inner;
        }

        /// <inheritdoc />
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

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

        /// <summary>Returns the inner projection by removing the array wrapping.</summary>
        public Projection Unfold() => _inner;

        /// <inheritdoc />
        public override Projection Lower() => Empty;
    }
}
