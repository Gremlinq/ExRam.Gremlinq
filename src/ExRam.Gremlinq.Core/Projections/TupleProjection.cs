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
            var projectionTraversals = _projections
                .Select((projection, i) => projection.Projection
                    .ToTraversal(environment)
                    .Prepend(new SelectStep(projection.Key))
                    .ToImmutableArray())
                .ToArray();

            if (projectionTraversals.All(x => x.Length == 1))
                return Traversal.Empty;

            return projectionTraversals
                .Select(traversal => (Step)new ProjectStep.ByTraversalStep(traversal))
                .Prepend(new ProjectStep(_projections
                    .Select(x => x.Key)
                    .ToImmutableArray()))
                .ToImmutableArray();
        }

        public override Projection BaseProjection => Empty;
    }
}
