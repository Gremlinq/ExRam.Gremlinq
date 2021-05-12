using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public static readonly OutEStep Empty = new(ImmutableArray<string>.Empty);

        public OutEStep(QuerySemantics? semantics = default) : this(ImmutableArray<string>.Empty, semantics)
        {
        }

        public OutEStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels,semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new OutEStep(semantics);
    }
}
