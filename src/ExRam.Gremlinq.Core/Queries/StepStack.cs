using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    public readonly struct StepStack : IReadOnlyList<Step>
    {
        public static readonly StepStack Empty = new(Array.Empty<Step>(), 0, default);

        private readonly Step?[] _steps;
        private readonly QuerySemantics _initialSemantics;

        internal StepStack(Step?[] steps, int count, QuerySemantics initialSemantics)
        {
            Count = count;
            _initialSemantics = initialSemantics;
            _steps = steps;
        }

        public StepStack Push(Step step)
        {
            if (_steps is { } steps)
            {
                var newSteps = _steps;

                if (Count < steps.Length)
                {
                    if (Interlocked.CompareExchange(ref _steps[Count], step, default) != null)
                        newSteps = new Step[_steps.Length];
                }
                else
                    newSteps = new Step[Math.Max(_steps.Length * 2, 16)];

                if (newSteps != _steps)
                {
                    Array.Copy(_steps, newSteps, Count);
                    newSteps[Count] = step;
                }

                return new StepStack(
                    newSteps,
                    Count + 1,
                    _initialSemantics);
            }

            throw new InvalidOperationException();
        }

        public StepStack Pop() => Pop(out _);

        public StepStack Pop(out Step poppedStep)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            poppedStep = _steps[Count - 1]!;
            return new StepStack(_steps, Count - 1, _initialSemantics);
        }

        public StepStack OverrideSemantics(QuerySemantics semantics) => IsEmpty
            ? new StepStack(_steps, Count, semantics)
            : Pop().Push(Peek().OverrideQuerySemantics(semantics));

        public (QuerySemantics semantics, int index) GetProjectionIndex()
        {
            var index = Count;

            for (var i = Count - 1; i >= 0; i--)
            {
                if (this[i].Semantics is { } semantics)
                {
                    if (!typeof(IArrayGremlinQueryBase).IsAssignableFrom(semantics.QueryType))
                        return (semantics, index);

                    index = i;
                }
            }

            return (_initialSemantics, 0);
        }

        public IEnumerator<Step> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return _steps[i]!;
            }
        }

        internal bool IsEmpty
        {
            get => Count == 0;
        }

        internal QuerySemantics Semantics
        {
            get
            {
                for (var i = Count - 1; i >= 0; i--)
                {
                    if (this[i].Semantics is { } semantics)
                        return semantics;
                }

                return _initialSemantics;
            }
        }

        internal Step Peek() => PeekOrDefault() ?? throw new InvalidOperationException();

        internal Step? PeekOrDefault() => Count > 0 ? _steps[Count - 1] : null;

        internal void CopyTo(Step[] destination, int sourceIndex, int destinationIndex, int count) => Array.Copy(_steps, sourceIndex, destination, destinationIndex, count);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public Step this[int index]
        {
            get => index < 0 || index >= Count ? throw new ArgumentOutOfRangeException() : _steps[index]!;
        }
    }
}
