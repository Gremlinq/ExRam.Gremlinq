using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class TupleProjection : Projection
    {
        private readonly (string Key, Projection Projection)[] _projections;

        internal TupleProjection((string Key, Projection Projection)[] projections)
        {
            _projections = projections;
        }
              
        public Projection Select(ImmutableArray<Key> keys)
        {
            var projections = _projections
                .Where(x => keys.Contains(x.Key))
                .ToArray();

            return projections.Length switch
            {
                0 => Empty,
                1 => projections[0].Projection,
                _ => new TupleProjection(projections)
            };
        }

        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            var emptyProjectionProtection = environment.Options.GetValue(GremlinqOption.EnableEmptyProjectionValueProtection)
                ? environment.Options.GetValue(GremlinqOption.EmptyProjectionProtectionDecoratorSteps)
                : default(Traversal?);

            var projectionTraversals = _projections
                .Select(projection =>
                {
                    var projectionTraversal = projection.Projection
                        .ToTraversal(environment);

                    return Traversal.Create(
                        emptyProjectionProtection is not null
                            ? projectionTraversal.Count + 2
                            : projectionTraversal.Count + 1,
                        (projection, projectionTraversal, emptyProjectionProtection),
                        static (steps, state) =>
                        {
                            var (projection, projectionTraversal, emptyProjectionProtection) = state;

                            steps[0] = new SelectKeysStep(projection.Key);

                            projectionTraversal.Steps
                                .AsSpan()
                                .CopyTo(steps[1..]);

                            if (emptyProjectionProtection is not null)
                                steps[^1] = FoldStep.Instance;
                        });
                })
                .ToArray();

            if (projectionTraversals.All(static x => x.Count == 1))
                return Traversal.Empty;

            var projectStep = new ProjectStep(_projections
                .Select(static x => x.Key)
                .ToImmutableArray());

            return Traversal.Create(
                emptyProjectionProtection is { } protection
                    ? projectionTraversals.Length + protection.Count + 1
                    : projectionTraversals.Length + 1,
                (projectStep, projectionTraversals, emptyProjectionProtection),
                static (steps, state) =>
                {
                    var (projectStep, projectionTraversals, emptyProjectionProtection) = state;

                    steps[0] = projectStep;

                    for (var i = 0; i < projectionTraversals.Length; i++)
                    {
                        steps[i + 1] = new ProjectStep.ByTraversalStep(projectionTraversals[i]);
                    }

                    if (emptyProjectionProtection is { } protection)
                    {
                        protection.Steps
                            .AsSpan()
                            .CopyTo(steps[^protection.Count..]);
                    }
                });
        }

        public override Projection Lower() => Empty;
    }
}
