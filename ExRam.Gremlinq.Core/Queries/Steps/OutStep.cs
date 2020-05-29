using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public static readonly OutStep Empty = new OutStep(ImmutableArray<string>.Empty);

        public OutStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
