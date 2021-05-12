using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep Empty = new(ImmutableArray<string>.Empty);

        public InStep(QuerySemantics? semantics = default) : this(ImmutableArray<string>.Empty, semantics)
        {
        }

        public InStep(ImmutableArray<string> labels, QuerySemantics? semantics = default) : base(labels, semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new InStep(Labels, semantics);
    }
}
