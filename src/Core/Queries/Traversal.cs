using System.Buffers;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// An immutable sequence of Gremlin <see cref="Steps.Step"/> objects that represents a traversal, along with its projection.
    /// </summary>
    public readonly struct Traversal
    {
        /// <summary>
        /// An empty traversal with no steps.
        /// </summary>
        public static readonly Traversal Empty = new(FastImmutableList<Step>.Empty, 0, Projection.Empty);

        private readonly uint _writeStepsCount;
        private readonly FastImmutableList<Step> _steps;

        private Traversal(FastImmutableList<Step> steps, Projection projection) : this(steps, SideEffectSemanticsHelper(steps.AsSpan()), projection)
        {
        }

        private Traversal(FastImmutableList<Step> steps, uint writeStepsCount, Projection projection)
        {
            _steps = steps;
            Projection = projection;
            _writeStepsCount = writeStepsCount;
        }

        /// <summary>Returns a new traversal with the given steps appended.</summary>
        /// <param name="steps">The steps to append.</param>
        public Traversal Push(params ReadOnlySpan<Step> steps) => new(
            _steps.Push(steps),
            _writeStepsCount + SideEffectSemanticsHelper(steps),
            Projection);

        /// <summary>Returns a new traversal with the last step removed.</summary>
        public Traversal Pop() => Pop(out _);

        /// <summary>Returns a new traversal with the last step removed and outputs that step.</summary>
        /// <param name="poppedStep">The removed step.</param>
        public Traversal Pop(out Step poppedStep)
        {
            var newSteps = _steps.Pop(out poppedStep);

            return new Traversal(
                newSteps,
                poppedStep.SideEffectSemanticsChange == SideEffectSemanticsChange.Write
                    ? _writeStepsCount - 1
                    : _writeStepsCount,
                Projection);
        }

        /// <summary>Returns a sub-traversal starting at the given index.</summary>
        /// <param name="start">The zero-based start index.</param>
        public Traversal Slice(int start) => this[start..];

        /// <summary>Returns a sub-traversal starting at the given index with the given length.</summary>
        /// <param name="start">The zero-based start index.</param>
        /// <param name="length">The number of steps to include.</param>
        public Traversal Slice(int start, int length) => new (_steps.Slice(start, length), Projection);

        /// <summary>Returns a new traversal with the given projection.</summary>
        /// <param name="projection">The new projection.</param>
        public Traversal WithProjection(Projection projection)
        {
            ArgumentNullException.ThrowIfNull(projection);

            return new(_steps, _writeStepsCount, projection);
        }

        /// <summary>Appends the projection's steps to the traversal and clears the projection.</summary>
        /// <param name="environment">The query environment.</param>
        public Traversal IncludeProjection(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            if (Projection != Projection.Empty)
            {
                var projectionTraversal = Projection.ToTraversal(environment);

                if (projectionTraversal.Count > 0)
                {
                    var newSteps = FastImmutableList<Step>
                        .Create(
                            Count + projectionTraversal.Count,
                            (_steps, projectionTraversal),
                            static (newSteps, state) =>
                            {
                                var (steps, projectionTraversal) = state;

                                steps
                                    .AsSpan()
                                    .CopyTo(newSteps);

                                projectionTraversal
                                    .Steps
                                    .CopyTo(newSteps[steps.Count..]);
                            });

                    return new Traversal(newSteps, _writeStepsCount, Projection.Empty);
                }
            }

            return this;
        }

        /// <summary>Implicitly wraps a single step into a traversal.</summary>
        /// <param name="step">The step to wrap.</param>
        public static implicit operator Traversal(Step step) => Create(1, step, static (span, step) => span[0] = step);

        /// <summary>Creates a traversal by populating a span of steps with a delegate.</summary>
        /// <typeparam name="TState">The type of the state passed to the delegate.</typeparam>
        /// <param name="length">The number of steps.</param>
        /// <param name="state">State passed to the creation delegate.</param>
        /// <param name="action">The delegate that populates the step span.</param>
        public static Traversal Create<TState>(int length, TState state, SpanAction<Step, TState> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            return new(
                FastImmutableList<Step>.Create(length, state, action),
                Projection.Empty);
        }

        /// <summary>Gets the number of steps in this traversal.</summary>
        public int Count => _steps.Count;

        /// <summary>Gets the projection describing the expected result shape.</summary>
        public Projection Projection { get; }

        /// <summary>Gets the step at the specified index.</summary>
        /// <param name="index">The zero-based index.</param>
        public Step this[int index] => Steps[index];

        /// <summary>Gets the side-effect semantics of this traversal (read or write).</summary>
        public SideEffectSemantics SideEffectSemantics => _writeStepsCount > 0
            ? SideEffectSemantics.Write
            : SideEffectSemantics.Read;

        /// <summary>Gets the steps as a read-only span.</summary>
        public ReadOnlySpan<Step> Steps => _steps.AsSpan();

        private static uint SideEffectSemanticsHelper(ReadOnlySpan<Step> steps)
        {
            var ret = 0U;

            for (var i = 0; i < steps.Length; i++)
            {
                if (steps[i] is { } step)
                {
                    if (step.SideEffectSemanticsChange == SideEffectSemanticsChange.Write)
                        ret++;
                }
                else
                    throw new ArgumentNullException(nameof(steps));
            }

            return ret;
        }
    }
}
