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

                            steps[2] = new GroupStep.ByTraversalStep(Traversal
                                .Create(
                                    keyProjectionTraversal.Count + 1,
                                    keyProjectionTraversal,
                                    static (steps, keyProjectionTraversal) =>
                                    {
                                        steps[0] = new SelectColumnStep(Column.Keys);

                                        keyProjectionTraversal.Steps
                                            .CopyTo(steps[1..]);
                                    }));

                            steps[3] = maybeValueProjectionTraversal is { } valueProjectionTraversal
                                ? new GroupStep.ByTraversalStep(Traversal
                                    .Create(
                                        valueProjectionTraversal.Count + 3,
                                        valueProjectionTraversal,
                                        static (steps, valueProjectionTraversal) =>
                                        {
                                            steps[0] = new SelectColumnStep(Column.Values);
                                            steps[1] = UnfoldStep.Instance;
                                            
                                            valueProjectionTraversal.Steps
                                                .CopyTo(steps[2..]);

                                            steps[^1] = FoldStep.Instance;
                                        }))
                                    : new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Values));
                        }));
        }

        public override Projection Lower() => Empty;
    }
}
