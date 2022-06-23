using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        public static readonly Traversal Empty = new(Array.Empty<Step>(), Projection.Empty);

        private readonly Step?[]? _steps;

        internal Traversal(Step?[] steps, Projection projection) : this(steps, steps.Length, projection)
        {

        }

        internal Traversal(Step?[] steps, int count, Projection projection) : this(steps, count, SideEffectSemanticsHelper(steps, count), projection)
        {
            
        }

        internal Traversal(Step?[] steps, int count, SideEffectSemantics semantics, Projection projection)
        {
            Count = count;
            _steps = steps;
            Projection = projection;
            SideEffectSemantics = semantics;
        }

        public Traversal Push(params Step[] steps)
        {
            var ret = this
                .EnsureCapacity(Count + steps.Length);

            for(var i = 0; i < steps.Length; i++)
            {
                ret = ret.Push(steps[i]);
            }

            return ret;
        }

        public Traversal Push(Step step)
        {
            var steps = Steps;

            if (Count < steps.Length)
            {
                if (Interlocked.CompareExchange(ref steps[Count], step, default) != null)
                    return Clone().Push(step);

                return new Traversal(
                    steps,
                    Count + 1,
                    step.SideEffectSemanticsChange == SideEffectSemanticsChange.Write
                        ? SideEffectSemantics.Write
                        : SideEffectSemantics,
                    Projection);
            }
            else
                return EnsureCapacity(Math.Max(steps.Length * 2, 16)).Push(step);
        }

        public Traversal Pop() => Pop(out _);

        public Traversal Pop(out Step poppedStep)
        {
            if (Count == 0)
                throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

            poppedStep = this[Count - 1];
            return new Traversal(Steps, Count - 1, Projection);
        }

        public Traversal WithProjection(Projection projection) => new(Steps, Count, projection);

        public IEnumerator<Step> GetEnumerator()
        {
            var steps = Steps;

            for (var i = 0; i < Count; i++)
            {
                yield return steps[i]!;
            }
        }

        public Traversal IncludeProjection(IGremlinQueryEnvironment environment)
        {
            if (Projection != Projection.Empty)
            {
                var projectionTraversal = Projection.ToTraversal(environment);

                if (projectionTraversal.Count > 0)
                {
                    var ret = new Step[Count + projectionTraversal.Count];

                    CopyTo(ret, 0);
                    projectionTraversal.CopyTo(ret, Count);

                    return new Traversal(ret, Projection.Empty);
                }
            }

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public Projection Projection { get; }

        public Step this[int index]
        {
            get => index < 0 || index >= Count
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : Steps[index]!;
        }

        public SideEffectSemantics SideEffectSemantics { get; }

        public void CopyTo(Step[] destination) => CopyTo(destination, 0);

        public void CopyTo(Step[] destination, int destinationIndex) => CopyTo(0, destination, destinationIndex, Count);

        public void CopyTo(int sourceIndex, Step[] destination, int destinationIndex, int length)
        {
            if (length + sourceIndex > Count)
                throw new ArgumentException();

            Array.Copy(Steps, sourceIndex, destination, destinationIndex, length);
        }

        internal Step Peek() => PeekOrDefault() ?? throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

        internal Step? PeekOrDefault() => Count > 0 ? this[Count - 1] : null;

        public static implicit operator Traversal(Step step) => new(new[] { step }, Projection.Empty);

        private static SideEffectSemantics SideEffectSemanticsHelper(Step?[] steps, int count)
        {
            for (var i = 0; i < count; i++)
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

        private Traversal EnsureCapacity(int count)
        {
            if (Steps.Length < count)
            {
                var newSteps = new Step[count];
                Array.Copy(Steps, newSteps, Count);

                return new(newSteps, Count, SideEffectSemantics, Projection);
            }

            return this;
        }

        private Traversal Clone()
        {
            var newSteps = new Step[Steps.Length];
            Array.Copy(Steps, newSteps, Count);

            return new(newSteps, Count, SideEffectSemantics, Projection);
        }

        private Step?[] Steps => _steps ?? Array.Empty<Step>();

        internal bool IsEmpty { get => Count == 0; }
    }
}
