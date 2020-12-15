using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep Empty = new(ImmutableArray<string>.Empty);

        public InStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
