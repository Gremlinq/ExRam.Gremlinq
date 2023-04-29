using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public static readonly OutStep NoLabels = new(ImmutableArray<string>.Empty);

        public OutStep() : base(ImmutableArray<string>.Empty)
        {
        }

        public OutStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
