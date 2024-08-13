using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public static readonly BothEStep NoLabels = new();

        //TODO: Think about making this private to force use of NoLabels.
        public BothEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public BothEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
