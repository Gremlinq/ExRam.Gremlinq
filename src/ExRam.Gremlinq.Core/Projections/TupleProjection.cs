using System.Collections.Immutable;
using System.Linq;
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

                            projectionTraversal
                                .AsSpan()
                                .CopyTo(steps[1..]);

                            if (emptyProjectionProtection is not null)
                                steps[^1] = FoldStep.Instance;
                        });
                })
                .ToArray();

            if (projectionTraversals.All(static x => x.Count == 1))
                return Traversal.Empty;

            return Traversal.Create(
                projectionTraversals.Length + 1 + (emptyProjectionProtection is { } protection
                    ? protection.Count
                    : 0),
                (_projections, projectionTraversals, emptyProjectionProtection),
                static (steps, state) =>
                {
                    var (projections, projectionTraversals, emptyProjectionProtection) = state;

                    steps[0] = new ProjectStep(projections
                        .Select(static x => x.Key)
                        .ToImmutableArray());

                    for (var i = 0; i < projectionTraversals.Length; i++)
                    {
                        steps[i + 1] = new ProjectStep.ByTraversalStep(projectionTraversals[i]);
                    }

                    if (emptyProjectionProtection is { } protection)
                    {
                        protection
                            .AsSpan()
                            .CopyTo(steps[(projectionTraversals.Length + 1)..]);
                    }
                });
        }

        public override Projection Lower() => Empty;
    }
}
