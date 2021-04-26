using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ExRam.Gremlinq.Core
{
    public struct StepStack : IReadOnlyList<Step>
    {
        public static readonly StepStack Empty = new(Array.Empty<Step>(), 0);

        private readonly int _count;
        private readonly Step?[] _steps;

        internal StepStack(Step?[] steps, int count)
        {
            _steps = steps;
            _count = count;
        }

        internal bool IsEmpty
        {
            get => Count == 0;
        }

        internal Step? Peek() => _count > 0 ? _steps[_count - 1] : null;

        public StepStack Push(Step step)
        {
            if (_steps is { } steps)
            {
                if (_count < steps.Length)
                {
                    if (Interlocked.CompareExchange(ref _steps[_count], step, default) == null)
                        return new StepStack(_steps, _count + 1);

                    var newSteps = new Step[_steps.Length];
                    Array.Copy(_steps, newSteps, _count);
                    newSteps[_count] = step;

                    return new StepStack(newSteps, _count + 1);
                }
                else
                {
                    var newSteps = new Step[Math.Max(_steps.Length * 2, 16)];
                    Array.Copy(_steps, newSteps, _count);
                    newSteps[_count] = step;

                    return new StepStack(newSteps, _count + 1);
                }
            }

            return Empty.Push(step);
        }

        public StepStack Pop()
        {
            return Pop(out _);
        }

        public StepStack Pop(out Step poppedStep)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            poppedStep = _steps[_count - 1]!;
            return new StepStack(_steps, _count - 1);
        }

        public IEnumerator<Step> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _steps[i]!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get => _count; }

        public Step this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException();

                return _steps[index]!;
            }
        }
    }
}
