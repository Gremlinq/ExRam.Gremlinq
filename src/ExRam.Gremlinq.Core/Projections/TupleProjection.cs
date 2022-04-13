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
                .Select(projection => projection.Projection
                    .ToTraversal(environment)
                    .Prepend(new SelectKeysStep(projection.Key))
                    .Apply(
                        static (traversal, emptyProjectionProtection) => emptyProjectionProtection is { } protection
                            ? traversal.Append(FoldStep.Instance)
                            : traversal,
                        emptyProjectionProtection)
                    .ToImmutableArray())
                .ToArray();

            if (projectionTraversals.All(x => x.Length == 1))
                return Traversal.Empty;

            return projectionTraversals
                .Select(traversal => (Step)new ProjectStep.ByTraversalStep(traversal))
                .Prepend(new ProjectStep(_projections
                    .Select(x => x.Key)
                    .ToImmutableArray()))
                .Apply(
                    static (traversal, emptyProjectionProtection) => emptyProjectionProtection is { } protection
                        ? traversal.Concat(emptyProjectionProtection)
                        : traversal,
                    emptyProjectionProtection)
                .ToImmutableArray();
        }

        public override Projection Lower() => Empty;
    }
}
