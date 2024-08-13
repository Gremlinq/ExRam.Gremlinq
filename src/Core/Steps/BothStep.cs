using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
        public static readonly BothStep NoLabels = new();

        public BothStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public BothStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
