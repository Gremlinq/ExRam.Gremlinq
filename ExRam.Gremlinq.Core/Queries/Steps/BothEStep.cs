using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public static readonly BothEStep NoLabels = new BothEStep(ImmutableArray<string>.Empty);

        public BothEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
