using System.Buffers;
using System.Collections;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        public static readonly Traversal Empty = new(FastImmutableList<Step>.Empty, SideEffectSemantics.Read, Projection.Empty);

        internal Traversal(Step[] steps, Projection projection) : this(new FastImmutableList<Step>(steps, steps.Length), projection)
        {
        }

        internal Traversal(FastImmutableList<Step> steps, Projection projection) : this(steps, SideEffectSemanticsHelper(steps.AsSpan()), projection)
        {
        }

        internal Traversal(FastImmutableList<Step> steps, SideEffectSemantics semantics, Projection projection)
        {
            Steps = steps;
            Projection = projection;
            SideEffectSemantics = semantics;
        }

        public Traversal Push(params Step[] steps)
        {
            return new Traversal(
                Steps.Push(steps),
                SideEffectSemanticsHelper(steps.AsSpan()) == SideEffectSemantics.Write
                    ? SideEffectSemantics.Write
                    : SideEffectSemantics,
                Projection);
        }

        public Traversal Push(Step step)
        {
            return new Traversal(
                Steps.Push(step),
                step.SideEffectSemanticsChange == SideEffectSemanticsChange.Write
                    ? SideEffectSemantics.Write
                    : SideEffectSemantics,
                Projection);
        }

        public Traversal Pop() => Pop(out _);

        public Traversal Pop(out Step poppedStep)
        {
            var newSteps = Steps.Pop(out poppedStep);

            return new Traversal(newSteps, SideEffectSemantics, Projection);    //TODO: SideEffectSemantics may change on Pop.
        }

        public Traversal WithProjection(Projection projection) => new(Steps, SideEffectSemantics, projection);

        public IEnumerator<Step> GetEnumerator() => Steps.GetEnumerator();

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
                            (Steps, projectionTraversal),
                            static (newSteps, state) =>
                            {
                                var (steps, projectionTraversal) = state;

                                steps
                                    .AsSpan()
                                    .CopyTo(newSteps);

                                projectionTraversal
                                    .Steps
                                    .AsSpan()
                                    .CopyTo(newSteps[steps.Count..]);
                            });

                    return new Traversal(newSteps, SideEffectSemantics, Projection.Empty);
                }
            }

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator Traversal(Step step) => new(new[] { step }, Projection.Empty);

        public static Traversal Create<TState>(int length, TState state, SpanAction<Step, TState> action) => new(
            FastImmutableList<Step>.Create(length, state, action),
            Projection.Empty);

        public int Count => Steps.Count;

        public Projection Projection { get; }

        public Step this[int index] => Steps[index];

        public SideEffectSemantics SideEffectSemantics { get; }

        public FastImmutableList<Step> Steps { get; }

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
    }
}
