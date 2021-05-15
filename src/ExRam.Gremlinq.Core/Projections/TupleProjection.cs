using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class TupleProjection : Projection
    {
        private readonly ProjectStep _projectStep;
        private readonly ImmutableList<Projection> _projections;

        private TupleProjection(ProjectStep projectStep) : this(projectStep, ImmutableList<Projection>.Empty)
        {

        }

        private TupleProjection(ProjectStep projectStep, ImmutableList<Projection> projections)
        {
            _projections = projections;
            _projectStep = projectStep;
        }

        public static TupleProjection Create(ProjectStep step)
        {
            return new TupleProjection(step);
        }

        public Projection Select(ImmutableArray<Key> keys)
        {
            var projectionKeys = new List<string>();
            var projections = ImmutableList<Projection>.Empty;

            for (var i = 0; i < _projectStep.Projections.Length; i++)
            {
                if (keys.Contains(_projectStep.Projections[i]))
                {
                    projectionKeys.Add(_projectStep.Projections[i]);
                    projections = projections.Add(_projections[i]);
                }
            }

            return projectionKeys.Count switch
            {
                0 => None,
                1 => projections[0],
                _ => new TupleProjection(
                        new ProjectStep(projectionKeys.ToImmutableArray()),
                        projections)
            };
        }

        public TupleProjection Add(ProjectStep.ByStep step)
        {
            if (_projections.Count >= _projectStep.Projections.Length)
                throw new InvalidOperationException();

            var newProjection = step is ProjectStep.ByTraversalStep byTraversal
                ? byTraversal.Traversal.Projection
                : None;

            return new TupleProjection(
                _projectStep,
                _projections.Add(newProjection));
        }

        public override Traversal Expand(IGremlinQueryEnvironment environment)
        {
            if (_projections.Count != _projectStep.Projections.Length)
                throw new InvalidOperationException();

            var projectionTraversals = _projections
                .Select((projection, i) => projection
                    .Expand(environment)
                    .Prepend(new SelectStep(_projectStep.Projections[i]))
                    .ToImmutableArray())
                .ToArray();

            if (projectionTraversals.All(x => x.Length == 1))
                return Traversal.Empty;

            return projectionTraversals
                .Select(traversal => (Step)new ProjectStep.ByTraversalStep(traversal))
                .Prepend(_projectStep)
                .ToImmutableArray();
        }

        public override Projection BaseProjection => None;
    }
}
