using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public static readonly BothEStep NoLabels = new(ImmutableArray<string>.Empty);
        
        public BothEStep(QuerySemantics? semantics = default) : this(ImmutableArray<string>.Empty, semantics)
        {
        }

        public BothEStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels, semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new BothEStep(semantics);
    }
}
