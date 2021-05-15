using System;

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

        public abstract Traversal Expand(IGremlinQueryEnvironment environment);

        public ArrayProjection ToArray() => new(this);

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
