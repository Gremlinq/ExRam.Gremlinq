using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        public static readonly Traversal Empty = new(Array.Empty<Step>(), Projection.Empty);

        private readonly int _count;
        private readonly Step?[]? _steps;

        internal Traversal(IEnumerable<Step> steps, Projection projection) : this(ToArrayHelper(steps), projection)
        {
        }

        internal Traversal(Step?[] steps, Projection projection) : this(steps, steps.Length, projection)
        {

        }

        internal Traversal(Step?[] steps, int count, Projection projection) : this(steps, count, SideEffectSemanticsHelper(steps, count), projection)
        {
            
        }

        internal Traversal(Step?[] steps, int count, SideEffectSemantics semantics, Projection projection)
        {
            _count = count;
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

            if (_count < steps.Length)
            {
                if (Interlocked.CompareExchange(ref steps[_count], step, default) != null)
                    return Clone().Push(step);

                return new Traversal(
                    steps,
                    _count + 1,
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
            return new Traversal(_steps!, Count - 1, Projection);
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

        public int Count { get => Steps is not null ? _count : 0; }

        public Projection Projection { get; }

        public Step this[int index]
        {
            get => index < 0 || index >= Count
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : _steps![index]!;
        }

        public SideEffectSemantics SideEffectSemantics { get; }

        public void CopyTo(Step[] destination) => CopyTo(destination, 0);

        public void CopyTo(Step[] destination, int destinationIndex) => CopyTo(0, destination, destinationIndex, Count);

        public void CopyTo(int sourceIndex, Step[] destination, int destinationIndex, int length)
        {
            if (length + sourceIndex > _count)
                throw new ArgumentException();

            Array.Copy(Steps, sourceIndex, destination, destinationIndex, length);
        }

        internal Step Peek() => PeekOrDefault() ?? throw new InvalidOperationException($"{nameof(Traversal)} is Empty.");

        internal Step? PeekOrDefault() => Count > 0 ? this[Count - 1] : null;

        public static implicit operator Traversal(Step step) => new(new[] { step }, Projection.Empty);

        private static Step[] ToArrayHelper(IEnumerable<Step> steps) => steps is Step[] array
            ? (Step[])array.Clone()
            : steps.ToArray();

        private static SideEffectSemantics SideEffectSemanticsHelper(Step?[] steps, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (steps[i]!.SideEffectSemanticsChange == SideEffectSemanticsChange.Write)
                {
                    return SideEffectSemantics.Write;
                }
            }

            return SideEffectSemantics.Read;
        }

        private Traversal EnsureCapacity(int count)
        {
            if (_steps!.Length < count)
            {
                var newSteps = new Step[count];
                Array.Copy(_steps!, newSteps, _count);

                return new(newSteps, _count, SideEffectSemantics, Projection);
            }

            return this;
        }

        private Traversal Clone()
        {
            var newSteps = new Step[_steps!.Length];
            Array.Copy(_steps!, newSteps, _count);

            return new(newSteps, _count, SideEffectSemantics, Projection);
        }

        private Step?[] Steps
        {
            get => _steps is { } steps
                ? steps
                : throw new InvalidOperationException($"{nameof(Traversal)} has not been initialized.");
        }

        internal bool IsEmpty { get => Count == 0; }
    }
}
