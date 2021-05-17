using System;
using System.Linq;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    public abstract class Projection
    {
        public static readonly Projection Edge = new EdgeProjection();
        public static readonly Projection None = new EmptyProjection();
        public static readonly Projection Value = new ValueProjection();
        public static readonly Projection Vertex = new VertexProjection();
        public static readonly Projection Element = new ElementProjection();
        public static readonly Projection EdgeOrVertex = new EdgeOrVertexProjection();

        public abstract Traversal ToTraversal(IGremlinQueryEnvironment environment);

        public ArrayProjection Fold() => new(this);

        public TupleProjection Project(ProjectStep projectStep, ProjectStep.ByStep[] bySteps)
        {
            if (projectStep.Projections.Length != bySteps.Length)
                throw new ArgumentException();

            return new TupleProjection(projectStep.Projections
                .Select((key, i) =>
                {
                    var projection = bySteps[i] is ProjectStep.ByTraversalStep byTraversal
                        ? byTraversal.Traversal.Projection
                        : None;

                    return (key, projection);
                })
                .ToArray());
        }

        internal Projection If<TProjection>(Func<TProjection, Projection> transformation)
            where TProjection : Projection
        {
            if (this is TProjection projection)
                return transformation(projection);

            return this;
        }

        internal Projection Lowest(Projection other)
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

        internal Projection Highest(Projection other)
        {
            if (GetType().IsAssignableFrom(other.GetType()))
                return other;

            return this;
        }

        public abstract Projection BaseProjection { get; }

        public string Name { get => ToString()!; }
    }
}
