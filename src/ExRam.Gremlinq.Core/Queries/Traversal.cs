using System.Buffers;
using System.Collections;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        public static readonly Traversal Empty = new(Array.Empty<Step>(), Projection.Empty);

        private readonly FastImmutableList<Step> _steps;

        internal Traversal(Step[] steps, Projection projection) : this(steps, steps.Length, projection)
        {
        }

        internal Traversal(Step?[] steps, int count, Projection projection) : this(new FastImmutableList<Step>(steps, count), projection)
        {
        }

        internal Traversal(FastImmutableList<Step> steps, Projection projection) : this(steps, SideEffectSemanticsHelper(steps), projection)
        {
        }

        internal Traversal(FastImmutableList<Step> steps, SideEffectSemantics semantics, Projection projection)
        {
            _steps = steps;
            Projection = projection;
            SideEffectSemantics = semantics;
        }

        public Traversal Push(params Step[] steps)
        {
            return new Traversal(
                _steps.Push(steps),
                SideEffectSemanticsHelper(steps.AsSpan()) == SideEffectSemantics.Write
                    ? SideEffectSemantics.Write
                    : SideEffectSemantics,
                Projection);
        }

        public Traversal Push(Step step)
        {
            return new Traversal(
                _steps.Push(step),
                step.SideEffectSemanticsChange == SideEffectSemanticsChange.Write
                    ? SideEffectSemantics.Write
                    : SideEffectSemantics,
                Projection);
        }

        public Traversal Pop() => Pop(out _);

        public Traversal Pop(out Step poppedStep)
        {
            var newSteps = _steps.Pop(out poppedStep);

            return new Traversal(newSteps, SideEffectSemantics, Projection);    //TODO: SideEffectSemantics may change on Pop.
        }

        public Traversal WithProjection(Projection projection) => new(_steps, SideEffectSemantics, projection);

        public IEnumerator<Step> GetEnumerator() => _steps.GetEnumerator();

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
                                    .AsSpan()
                                    .CopyTo(newSteps[steps.Count..]);
                            });

                    return new Traversal(newSteps, SideEffectSemantics, Projection.Empty);
                }
            }

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get => _steps.Count; }

        public Projection Projection { get; }

        public Step this[int index]
        {
            get => _steps[index];
        }

        public SideEffectSemantics SideEffectSemantics { get; }

        public static implicit operator Traversal(Step step) => new(new[] { step }, Projection.Empty);

        public static Traversal Create<TState>(int length, TState state, SpanAction<Step, TState> action)
        {
            return new(
                FastImmutableList<Step>.Create(length, state, action),
                Projection.Empty);
        }

        public ReadOnlySpan<Step> AsSpan() => _steps.AsSpan()[..Count];

        public ReadOnlySpan<Step> AsSpan(Range range) => AsSpan()[range];

        public ReadOnlySpan<Step> AsSpan(int start, int length) => AsSpan().Slice(start, length);

        public ReadOnlySpan<Step> AsSpan(int start) => AsSpan()[start..];


        public ReadOnlyMemory<Step> AsMemory() => _steps.AsMemory()[..Count];

        public ReadOnlyMemory<Step> AsMemory(Range range) => AsMemory()[range];

        public ReadOnlyMemory<Step> AsMemory(int start, int length) => AsMemory().Slice(start, length);

        public ReadOnlyMemory<Step> AsMemory(int start) => AsMemory()[start..];

        private static SideEffectSemantics SideEffectSemanticsHelper(FastImmutableList<Step> steps) => SideEffectSemanticsHelper(steps.AsSpan());

        private static SideEffectSemantics SideEffectSemanticsHelper(ReadOnlySpan<Step> steps)
        {
            for (var i = 0; i < steps.Length; i++)
            {
                if (steps[i] is { } step)
                {
                    if (steps[i]!.SideEffectSemanticsChange == SideEffectSemanticsChange.Write)
                        return SideEffectSemantics.Write;
                }
                else
                    throw new ArgumentNullException(nameof(steps));
            }

            return SideEffectSemantics.Read;
        }

        internal bool IsEmpty { get => Count == 0; }
    }
}
