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
            _steps = steps;
            Count = count;
        }

        public StepStack Push(Step step)
        {
            if (_steps is { } steps)
            {
                var newSteps = _steps;

                if (Count < steps.Length)
                {
                    if (Interlocked.CompareExchange(ref _steps[Count], step, default) != null)
                    {
                        newSteps = new Step[_steps.Length];
                        Array.Copy(_steps, newSteps, Count);
                        newSteps[Count] = step;
                    }
                }
                else
                {
                    newSteps = new Step[Math.Max(_steps.Length * 2, 16)];
                    Array.Copy(_steps, newSteps, Count);
                    newSteps[Count] = step;
                }

                return new StepStack(newSteps, Count + 1);
            }

            return Empty.Push(step);
        }

        public StepStack Pop() => Pop(out _);

        public StepStack Pop(out Step poppedStep)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            poppedStep = _steps[Count - 1]!;
            return new StepStack(_steps, Count - 1);
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

        internal Step? Peek() => Count > 0 ? _steps[Count - 1] : null;

        internal Step? TryGetSingleStep() => !IsEmpty && Pop(out var step).IsEmpty
            ? step
            : default;

        internal Step? PeekOrDefault() => !IsEmpty
            ? Peek()
            : default;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public Step this[int index]
        {
            get => index < 0 || index >= Count ? throw new ArgumentOutOfRangeException() : _steps[index]!;
        }
    }
}
