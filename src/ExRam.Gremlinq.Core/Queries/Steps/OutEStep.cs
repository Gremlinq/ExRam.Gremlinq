using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public static readonly OutEStep Empty = new(ImmutableArray<string>.Empty);

        public OutEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
