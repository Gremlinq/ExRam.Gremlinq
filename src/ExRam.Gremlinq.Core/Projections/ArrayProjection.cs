using System.Collections.Immutable;
using System.Linq;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class ArrayProjection : Projection
    {
        internal ArrayProjection(Projection inner)
        {
            Inner = inner;
        }

        public override Traversal Expand(IGremlinQueryEnvironment environment)
        {
            var inner = Inner.Expand(environment);

            if (inner.Count > 0)
            {
                return new LocalStep(inner
                    .Prepend(UnfoldStep.Instance)
                    .Append(FoldStep.Instance)
                    .ToImmutableArray());
            }

            return Traversal.Empty;
        }

        public Projection Inner { get; }

        public override Projection BaseProjection => None;
    }
}
