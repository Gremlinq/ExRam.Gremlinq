using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Describes the expected shape of data returned by a traversal, used to optimize deserialization.</summary>
    public abstract class Projection
    {
        /// <summary>Gets the edge projection.</summary>
        public static readonly EdgeProjection Edge = new ();
        /// <summary>Gets the empty projection.</summary>
        public static readonly EmptyProjection Empty = new ();
        /// <summary>Gets the scalar value projection.</summary>
        public static readonly ValueProjection Value = new ();
        /// <summary>Gets the vertex projection.</summary>
        public static readonly VertexProjection Vertex = new ();
        /// <summary>Gets the element projection.</summary>
        public static readonly ElementProjection Element = new ();
        /// <summary>Gets the edge-or-vertex projection.</summary>
        public static readonly EdgeOrVertexProjection EdgeOrVertex = new ();
        /// <summary>Gets the vertex property projection.</summary>
        public static readonly VertexPropertyProjection VertexProperty = new();

        internal static readonly EmptyProjection Property = Empty;

        /// <summary>Converts this projection to a traversal that projects elements according to the current shape.</summary>
        /// <param name="environment">The query environment.</param>
        public virtual Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return Traversal.Empty;
        }

        /// <summary>Wraps this projection into an array projection.</summary>
        public ArrayProjection Fold() => new(this);

        /// <summary>Creates a tuple projection from the given project step and by-modulators.</summary>
        /// <param name="projectStep">The project step defining the projection keys.</param>
        /// <param name="bySteps">The by-modulator steps defining each projection.</param>
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

        /// <summary>Creates a group projection with the given key and value projections.</summary>
        /// <param name="keyProjection">The projection for group keys.</param>
        /// <param name="valueProjection">The projection for group values.</param>
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

        /// <summary>Returns the next lower projection in the projection hierarchy.</summary>
        public abstract Projection Lower();

        /// <summary>Gets the name of this projection.</summary>
        public string Name => ToString()!;
    }
}
