using System.Buffers;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal
    {
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

        public Traversal Push(params ReadOnlySpan<Step> steps) => new(
            _steps.Push(steps),
            _writeStepsCount + SideEffectSemanticsHelper(steps),
            Projection);

        public Traversal Pop() => Pop(out _);

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

        public Traversal Slice(int start) => this[start..];

        public Traversal Slice(int start, int length) => new (_steps.Slice(start, length), Projection);

        public Traversal WithProjection(Projection projection) => new(_steps, _writeStepsCount, projection);

        public Traversal IncludeProjection(IGremlinQueryEnvironment environment)
        {
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

        public static implicit operator Traversal(Step step) => Create(1, step, static (span, step) => span[0] = step);

        public static Traversal Create<TState>(int length, TState state, SpanAction<Step, TState> action) => new(
            FastImmutableList<Step>.Create(length, state, action),
            Projection.Empty);

        public int Count => _steps.Count;

        public Projection Projection { get; }

        public Step this[int index] => Steps[index];

        public SideEffectSemantics SideEffectSemantics => _writeStepsCount > 0
            ? SideEffectSemantics.Write
            : SideEffectSemantics.Read;

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
