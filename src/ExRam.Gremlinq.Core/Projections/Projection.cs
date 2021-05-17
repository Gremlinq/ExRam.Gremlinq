using System;
using System.Linq;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    public abstract class Projection
    {
        public static readonly EdgeProjection Edge = new ();
        public static readonly EmptyProjection Empty = new ();
        public static readonly ValueProjection Value = new ();
        public static readonly VertexProjection Vertex = new ();
        public static readonly ElementProjection Element = new ();
        public static readonly EdgeOrVertexProjection EdgeOrVertex = new ();
        public static readonly VertexPropertyProjection VertexProperty = new();

        internal static readonly EmptyProjection Property = Empty;

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
                        : Empty;

                    return (key, projection);
                })
                .ToArray());
        }

        public GroupProjection Group(Projection keyProjection, Projection valueProjection) => new GroupProjection(keyProjection, valueProjection);

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

            while (@this != Empty)
            {
                if (other.GetType().IsInstanceOfType(@this))
                    return other;

                if (@this.GetType().IsInstanceOfType(other))
                    return @this;

                @this = @this.Lower();
            }

            return Empty;
        }

        internal Projection Highest(Projection other)
        {
            if (GetType().IsAssignableFrom(other.GetType()))
                return other;

            return this;
        }

        public abstract Projection Lower();

        public string Name { get => ToString()!; }
    }
}
