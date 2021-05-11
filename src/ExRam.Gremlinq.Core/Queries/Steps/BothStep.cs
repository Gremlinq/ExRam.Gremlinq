using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
        public static readonly BothStep NoLabels = new(ImmutableArray<string>.Empty);

        public BothStep(QuerySemantics? semantics = default) : this(ImmutableArray<string>.Empty, semantics)
        {
        }

        public BothStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels, semantics)
        {
        }
    }
}
