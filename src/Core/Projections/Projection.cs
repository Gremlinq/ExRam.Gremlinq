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

        public virtual Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return Traversal.Empty;
        }

        public ArrayProjection Fold() => new(this);

        public TupleProjection Project(ProjectStep projectStep, ProjectStep.ByStep[] bySteps)
        {
            ArgumentNullException.ThrowIfNull(projectStep);
            ArgumentNullException.ThrowIfNull(bySteps);

            return Project(projectStep, bySteps.AsSpan());
        }

        internal TupleProjection Project(ProjectStep projectStep, ReadOnlySpan<ProjectStep.ByStep> bySteps)
        {
            if (projectStep.Projections.Length != bySteps.Length)
                throw new ArgumentException($"{nameof(projectStep)} must have the same number of projections as there are steps in {nameof(bySteps)}.");

            var tuples = new (string Key, Projection Projection)[projectStep.Projections.Length];

            for (var i = 0; i < tuples.Length; i++)
            {
                var projection = bySteps[i] is ProjectStep.ByTraversalStep byTraversal
                    ? byTraversal.Traversal.Projection
                    : Empty;

                tuples[i] = (projectStep.Projections[i], projection);
            }

            return new TupleProjection(tuples);
        }

        public GroupProjection Group(Projection keyProjection, Projection valueProjection)
        {
            ArgumentNullException.ThrowIfNull(keyProjection);
            ArgumentNullException.ThrowIfNull(valueProjection);

            return new(keyProjection, valueProjection);
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

            if (@this.IsHigherOrEqualThan(other))
                return other;

            while (@this != Empty)
            {
                if (other.IsHigherOrEqualThan(@this))
                    return @this;

                @this = @this.Lower();
            }

            return Empty;
        }

        internal Projection Highest(Projection other) => IsHigherOrEqualThan(other)
            ? this
            : other;

        private bool IsHigherOrEqualThan(Projection other)
        {
            var @this = this;
            var otherType = other.GetType();

            do
            {
                if (@this.GetType() == otherType)
                    return true;

                @this = @this.Lower();
            }
            while (@this != Empty);

            return false;
        }

        public abstract Projection Lower();

        public string Name => ToString()!;
    }
}
