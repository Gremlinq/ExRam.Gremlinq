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
            Count = count;
            _steps = steps;
            Projection = projection;
            SideEffectSemantics = semantics;
        }

        public Traversal Push(params Step[] steps)
        {
            //TODO: optimize!
            var ret = this;

            for(var i = 0; i < steps.Length; i++)
            {
                ret = ret.Push(steps[i]);
            }

            return ret;
        }

        public Traversal Push(Step step)
        {
            if (_steps is { } steps)
            {
                var newSteps = steps;

                if (Count < steps.Length)
                {
                    if (Interlocked.CompareExchange(ref steps[Count], step, default) != null)
                        newSteps = new Step[steps.Length];
                }
                else
                    newSteps = new Step[Math.Max(steps.Length * 2, 16)];

                if (newSteps != steps)
                {
                    Array.Copy(steps, newSteps, Count);
                    newSteps[Count] = step;
                }

                return new Traversal(
                    newSteps,
                    Count + 1,
                    step.SideEffectSemanticsChange == SideEffectSemanticsChange.Write
                        ? SideEffectSemantics.Write
                        : SideEffectSemantics,
                    Projection);
            }

            throw new InvalidOperationException();
        }

        public Traversal Pop() => Pop(out _);

        public Traversal Pop(out Step poppedStep)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            poppedStep = this[Count - 1];
            return new Traversal(_steps!, Count - 1, Projection);
        }

        public Traversal WithProjection(Projection projection)
        {
            return (_steps is { } steps)
                ? new (steps, Count, projection)
                : throw new InvalidOperationException();
        }

        public IEnumerator<Step> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return _steps![i]!;
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
                : _steps![index]!;
        }

        public SideEffectSemantics SideEffectSemantics { get; }

        public void CopyTo(Step[] destination) => CopyTo(destination, 0);

        public void CopyTo(Step[] destination, int destinationIndex) => CopyTo(0, destination, destinationIndex, Count);

        public void CopyTo(int sourceIndex, Step[] destination, int destinationIndex, int length)
        {
            if (_steps is Step[] source)
                Array.Copy(source, sourceIndex, destination, destinationIndex, length);
            else
                throw new InvalidOperationException();
        }

        internal Step Peek() => PeekOrDefault() ?? throw new InvalidOperationException();

        internal Step? PeekOrDefault() => Count > 0 ? this[Count - 1] : null;

        internal void CopyTo(Step[] destination, int sourceIndex, int destinationIndex, int count)
        {
            //TODO: Optimize
            for (var i = sourceIndex; i < count + sourceIndex; i++)
            {
                destination[destinationIndex++] = _steps![i]!;
            }
        }

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

        internal bool IsEmpty { get => Count == 0; }
    }
}
