using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
        public static readonly BothStep NoLabels = new(ImmutableArray<string>.Empty);

        public BothStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
