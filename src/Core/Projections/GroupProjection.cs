using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class GroupProjection : Projection
    {
        private readonly Projection _keyProjection;
        private readonly Projection _valueProjection;

        internal GroupProjection(Projection keyProjection, Projection valueProjection)
        {
            _keyProjection = keyProjection;
            _valueProjection = valueProjection;
        }

        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            var keyProjectionTraversal = _keyProjection.ToTraversal(environment);
            var valueProjectionTraversal = _valueProjection.ToTraversal(environment);

            return (keyProjectionTraversal.Count == 0 && valueProjectionTraversal.Count == 0)
                ? Traversal.Empty
                : new MapStep(Traversal
                    .Create(
                        4,
                        (keyProjectionTraversal, valueProjectionTraversal),
                        static (steps, state) =>
                        {
                            var (keyProjectionTraversal, valueProjectionTraversal) = state;

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

                            steps[3] = new GroupStep.ByTraversalStep(Traversal
                                .Create(
                                    valueProjectionTraversal.Count + 1,
                                    valueProjectionTraversal,
                                    static (steps, valueProjectionTraversal) =>
                                    {
                                        steps[0] = new SelectColumnStep(Column.Values);

                                        valueProjectionTraversal.Steps
                                            .CopyTo(steps[1..]);
                                    }));
                        }));
        }

        public override Projection Lower() => Empty;
    }
}
