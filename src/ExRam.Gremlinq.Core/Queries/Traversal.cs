using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    public readonly struct Traversal : IReadOnlyList<Step>
    {
        public static readonly Traversal Empty = new(ImmutableArray<Step>.Empty, true, Projection.Empty);

        private readonly IReadOnlyList<Step> _steps;

        public Traversal(IEnumerable<Step> steps, Projection projection) : this(ToArrayHelper(steps), true, projection)
        {
        }

        public Traversal(ImmutableArray<Step> steps, Projection projection) : this(steps, true, projection)
        {
        }

        internal Traversal(IReadOnlyList<Step> steps, bool owned, Projection projection)
        {
            _steps = owned
                ? steps
                : ToArrayHelper(steps);

            Projection = projection;
            SideEffectSemantics = SideEffectSemantics.Read;

            for (var i = 0; i < _steps.Count; i++)
            {
                if (_steps[i].SideEffectSemanticsChange == SideEffectSemanticsChange.Write)
                {
                    SideEffectSemantics = SideEffectSemantics.Write;

                    break;
                }
            }
        }

        public IEnumerator<Step> GetEnumerator() => _steps.GetEnumerator();

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

                    return new Traversal(ret, true, Projection.Empty);
                }
            }

            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count { get => _steps.Count; }

        public Projection Projection { get; }

        public Step this[int index] => _steps[index];

        public SideEffectSemantics SideEffectSemantics { get; }

        public void CopyTo(Step[] destination) => CopyTo(destination, 0);

        public void CopyTo(Step[] destination, int destinationIndex) => CopyTo(0, destination, destinationIndex, Count);

        public void CopyTo(int sourceIndex, Step[] destination, int destinationIndex, int length)
        {
            if (_steps is ImmutableArray<Step> immutableArray)
                immutableArray.CopyTo(sourceIndex, destination, destinationIndex, length);
            else if (_steps is Step[] source)
                Array.Copy(source, sourceIndex, destination, destinationIndex, length);
            else
                throw new InvalidOperationException();
        }

        public static implicit operator Traversal(Step[] steps) => new(steps, false, Projection.Empty);

        public static implicit operator Traversal(ImmutableArray<Step> steps) => new(steps, Projection.Empty);

        public static implicit operator Traversal(Step step) => new(new[] { step }, true, Projection.Empty);

        private static Step[] ToArrayHelper(IEnumerable<Step> steps) => steps is Step[] array
            ? (Step[])array.Clone()
            : steps.ToArray();
    }
}
