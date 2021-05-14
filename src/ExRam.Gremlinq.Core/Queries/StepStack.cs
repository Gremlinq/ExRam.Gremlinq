using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using StepTuple = System.Tuple<ExRam.Gremlinq.Core.Step, ExRam.Gremlinq.Core.QuerySemantics>;

namespace ExRam.Gremlinq.Core
{
    public readonly struct StepStack : IReadOnlyList<(Step Step, QuerySemantics Semantics)>
    {
        public static readonly StepStack Empty = new(Array.Empty<StepTuple>(), 0, default);

        private readonly StepTuple?[] _steps;

        internal StepStack(StepTuple?[] steps, int count, QuerySemantics initialSemantics)
        {
            Count = count;
            _steps = steps;         
            InitialSemantics = initialSemantics;
        }

        public StepStack Push(Step step, QuerySemantics? semantics = null)
        {
            if (_steps is { } steps)
            {
                var newSteps = _steps;
                var tuple = Tuple.Create(step, semantics ?? Semantics);

                if (Count < steps.Length)
                {
                    if (Interlocked.CompareExchange(ref _steps[Count], tuple, default) != null)
                        newSteps = new StepTuple[_steps.Length];
                }
                else
                    newSteps = new StepTuple[Math.Max(_steps.Length * 2, 16)];

                if (newSteps != _steps)
                {
                    Array.Copy(_steps, newSteps, Count);
                    newSteps[Count] = tuple;
                }

                return new StepStack(
                    newSteps,
                    Count + 1,
                    InitialSemantics);
            }

            throw new InvalidOperationException();
        }

        public StepStack Pop() => Pop(out _);

        public StepStack Pop(out (Step poppedStep, QuerySemantics poppedSemantics) tuple)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            tuple = this[Count - 1];
            return new StepStack(_steps, Count - 1, InitialSemantics);
        }

        public StepStack OverrideSemantics(QuerySemantics semantics)
        {
            if (IsEmpty)
                return new StepStack(_steps, Count, semantics);

            if (semantics == Semantics)
                return this;

            return Pop().Push(Peek().Step, semantics);
        }

        public IEnumerator<(Step Step, QuerySemantics Semantics)> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return this[i]!;
            }
        }

        internal (Step Step, QuerySemantics Semantics) Peek() => PeekOrDefault() ?? throw new InvalidOperationException();

        internal (Step Step, QuerySemantics Semantics)? PeekOrDefault() => Count > 0 ? this[Count - 1] : null;

        internal void CopyTo(Step[] destination, int sourceIndex, int destinationIndex, int count)
        {
            for(var i = sourceIndex; i < count + sourceIndex; i++)
            {
                destination[destinationIndex++] = _steps[i]!.Item1;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get; }

        public QuerySemantics Semantics
        {
            get
            {
                return Count > 0
                    ? this[Count - 1]!.Semantics
                    : InitialSemantics;
            }
        }

        public (Step Step, QuerySemantics Semantics) this[int index]
        {
            get => index < 0 || index >= Count ? throw new ArgumentOutOfRangeException(nameof(index)) : (_steps[index]!.Item1, _steps[index]!.Item2);
        }

        internal QuerySemantics InitialSemantics { get; }

        internal bool IsEmpty { get => Count == 0; }
    }
}
