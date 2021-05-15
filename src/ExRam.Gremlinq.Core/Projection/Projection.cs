using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    public abstract class Projection
    {
        #region Nested
        private sealed class _None : Projection
        {
            public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

            public override Projection BaseProjection => None;
        }

        private sealed class _Value : Projection
        {
            public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

            public override Projection BaseProjection => None;
        }

        private class _Element : Projection
        {
            public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

            public override Projection BaseProjection => None;
        }

        private class _EdgeOrVertex : _Element
        {
            public override Traversal Expand(IGremlinQueryEnvironment environment) => Traversal.Empty;

            public override Projection BaseProjection => Element;
        }

        private sealed class _Edge : _EdgeOrVertex
        {
            public override Traversal Expand(IGremlinQueryEnvironment environment) => environment.Options.GetValue(GremlinqOption.EdgeProjectionSteps);

            public override Projection BaseProjection => EdgeOrVertex;
        }

        private sealed class _Vertex : _EdgeOrVertex
        {
            public override Traversal Expand(IGremlinQueryEnvironment environment) => environment.Options.GetValue(environment.FeatureSet.Supports(VertexFeatures.MetaProperties)
                ? GremlinqOption.VertexProjectionSteps
                : GremlinqOption.VertexProjectionWithoutMetaPropertiesSteps);

            public override Projection BaseProjection => EdgeOrVertex;
        }

        private sealed class _VertexProperty : _Element
        {
            public override Projection BaseProjection => Element;
        }

        public sealed class Array : Projection
        {
            internal Array(Projection inner)
            {
                Inner = inner;
            }

            public override Traversal Expand(IGremlinQueryEnvironment environment)
            {
                var inner = Inner.Expand(environment);

                if (inner.Count > 0)
                {
                    return new LocalStep(inner
                        .Prepend(UnfoldStep.Instance)
                        .Append(FoldStep.Instance)
                        .ToImmutableArray());
                }

                return Traversal.Empty;
            }

            public Projection Inner { get; }

            public override Projection BaseProjection => None;
        }

        public sealed class Tuple : Projection
        {
            private readonly ProjectStep _projectStep;
            private readonly ImmutableList<Projection> _projections;

            private Tuple(ProjectStep projectStep) : this(projectStep, ImmutableList<Projection>.Empty)
            {

            }

            private Tuple(ProjectStep projectStep, ImmutableList<Projection> projections)
            {
                _projections = projections;
                _projectStep = projectStep;
            }

            public static Tuple Create(ProjectStep step)
            {
                return new Tuple(step);
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
                    _ => new Tuple(
                            new ProjectStep(projectionKeys.ToImmutableArray()),
                            projections)
                };
            }

            public Tuple Add(ProjectStep.ByStep step)
            {
                if (_projections.Count >= _projectStep.Projections.Length)
                    throw new InvalidOperationException();

                var newProjection = step is ProjectStep.ByTraversalStep byTraversal
                    ? byTraversal.Traversal.Projection
                    : None;

                return new Tuple(
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
        #endregion

        public static readonly Projection Edge = new _Edge();
        public static readonly Projection None = new _None();
        public static readonly Projection Value = new _Value();
        public static readonly Projection Vertex = new _Vertex();
        public static readonly Projection Element = new _Element();
        public static readonly Projection EdgeOrVertex = new _EdgeOrVertex();

        public abstract Traversal Expand(IGremlinQueryEnvironment environment);

        public Array ToArray() => new(this);

        public Projection If<TProjection>(Func<TProjection, Projection> transformation)
            where TProjection : Projection
        {
            if (this is TProjection projection)
                return transformation(projection);

            return this;
        }

        public Projection Lowest(Projection other)
        {
            var @this = this;

            while (@this != None)
            {
                if (other.GetType().IsInstanceOfType(@this))
                    return other;

                if (@this.GetType().IsInstanceOfType(other))
                    return @this;

                @this = @this.BaseProjection;
            }

            return None;
        }

        public Projection Highest(Projection other)
        {
            if (GetType().IsAssignableFrom(other.GetType()))
                return other;

            return this;
        }

        public abstract Projection BaseProjection { get; }

        public string Name { get => ToString()!; }
    }
}
