using System.Linq;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class GroupProjection : Projection
    {
        private readonly Projection _keyProjection;
        private readonly Projection? _valueProjection;

        internal GroupProjection(Projection keyProjection, Projection? valueProjection)
        {
            _keyProjection = keyProjection;
            _valueProjection = valueProjection;
        }

        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            var keyProjectionTraversal = _keyProjection.ToTraversal(environment);
            var maybeValueProjectionTraversal = _valueProjection?.ToTraversal(environment);

            return (keyProjectionTraversal.Count == 0 && (maybeValueProjectionTraversal?.Count).GetValueOrDefault() == 0)
                ? Traversal.Empty
                : new LocalStep(Traversal
                    .Create(
                        4,
                        (keyProjectionTraversal, maybeValueProjectionTraversal),
                        static (steps, state) =>
                        {
                            var (keyProjectionTraversal, maybeValueProjectionTraversal) = state;

                            steps[0] = UnfoldStep.Instance;
                            steps[1] = GroupStep.Instance;

                            steps[2] = new GroupStep.ByTraversalStep(keyProjectionTraversal
                                .Prepend(new SelectColumnStep(Column.Keys))
                                .ToTraversal());

                            steps[3] = maybeValueProjectionTraversal is { } valueProjectionTraversal
                                ? new GroupStep.ByTraversalStep(valueProjectionTraversal
                                    .Prepend(UnfoldStep.Instance)
                                    .Prepend(new SelectColumnStep(Column.Values))
                                    .Append(FoldStep.Instance)
                                    .ToTraversal())
                                : new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Values));
                        }));
        }

        public override Projection Lower() => Empty;
    }
}
