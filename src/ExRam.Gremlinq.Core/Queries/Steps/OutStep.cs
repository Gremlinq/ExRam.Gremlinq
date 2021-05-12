using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public static readonly OutStep Empty = new(ImmutableArray<string>.Empty);

        public OutStep(QuerySemantics? semantics = default) : base(ImmutableArray<string>.Empty, semantics)
        {
        }

        public OutStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels, semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new OutStep(semantics);
    }
}
