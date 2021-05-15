using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    public readonly struct StepStack : IReadOnlyList<Step>
    {
        public static readonly StepStack Empty = new(Array.Empty<Step>(), 0);

        private readonly Step?[] _steps;

        internal StepStack(Step?[] steps, int count)
        {
            Count = count;
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
                    Count + 1);
            }

            throw new InvalidOperationException();
        }

        public StepStack Pop() => Pop(out _);

        public StepStack Pop(out Step poppedStep)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            poppedStep = this[Count - 1];
            return new StepStack(_steps, Count - 1);
        }

        public Traversal ToTraversal(Projection projection)
        {
            var steps = new Step[Count];
            Array.Copy(_steps, steps, Count);

            return new Traversal(steps, true, projection);
        }

        public IEnumerator<Step> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return _steps[i]!;
            }
        }

        internal Step Peek() => PeekOrDefault() ?? throw new InvalidOperationException();

        internal Step? PeekOrDefault() => Count > 0 ? this[Count - 1] : null;

        internal void CopyTo(Step[] destination, int sourceIndex, int destinationIndex, int count)
        {
            for(var i = sourceIndex; i < count + sourceIndex; i++)
            {
                destination[destinationIndex++] = _steps[i]!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public Step this[int index]
        {
            get => index < 0 || index >= Count ? throw new ArgumentOutOfRangeException(nameof(index)) : _steps[index]!;
        }

        internal bool IsEmpty { get => Count == 0; }
    }
}
