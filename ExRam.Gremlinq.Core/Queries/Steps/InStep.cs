using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep Empty = new InStep(ImmutableArray<string>.Empty);

        public InStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
