using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for tuple (multi-column) results.</summary>
    public sealed class TupleProjection : Projection
    {
        private readonly (string Key, Projection Projection)[] _projections;

        internal TupleProjection((string Key, Projection Projection)[] projections)
        {
            _projections = projections;
        }

        /// <summary>Selects a subset of projections by key.</summary>
        /// <param name="keys">The keys to select.</param>
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

        /// <inheritdoc />
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            var emptyProjectionProtection = environment.Options.GetValue(GremlinqOption.EnableEmptyProjectionValueProtection)
                ? environment.Options.GetValue(GremlinqOption.EmptyProjectionProtectionDecoratorSteps)
                : default(Traversal?);

            var overhead = emptyProjectionProtection is not null
                ? 2
                : 1;

            var allEmpty = true;
            var projectionTraversals = new Traversal[_projections.Length];

            for (var i = 0; i < _projections.Length; i++)
            {
                var projectionTraversal = _projections[i].Projection
                    .ToTraversal(environment);

                projectionTraversals[i] = Traversal.Create(
                    projectionTraversal.Count + overhead,
                    (new SelectKeysStep(_projections[i].Key), projectionTraversal, emptyProjectionProtection),
                    static (steps, state) =>
                    {
                        var (selectKeysStep, projectionTraversal, emptyProjectionProtection) = state;

                        steps[0] = selectKeysStep;

                        projectionTraversal.Steps
                            .CopyTo(steps[1..]);

                        if (emptyProjectionProtection is not null)
                            steps[^1] = FoldStep.Instance;
                    });

                if (projectionTraversals[i].Count != overhead)
                    allEmpty = false;
            }

            if (allEmpty)
                return Traversal.Empty;

            var projectStep = CreateProjectStep();

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
                            .CopyTo(steps[^protection.Count..]);
                    }
                });
        }

        /// <inheritdoc />
        public override Projection Lower() => Empty;

        private ProjectStep CreateProjectStep()
        {
            var keys = new string[_projections.Length];

            for (var i = 0; i < keys.Length; i++)
            {
                keys[i] = _projections[i].Key;
            }

            return new ProjectStep(keys.UnsafeToImmutableArray());
        }
    }
}
