using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public static readonly InEStep Empty = new(ImmutableArray<string>.Empty);

        public InEStep(QuerySemantics? semantics = default) : this(ImmutableArray<string>.Empty, semantics)
        {
        }

        public InEStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels, semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new InEStep(semantics);
    }
}
