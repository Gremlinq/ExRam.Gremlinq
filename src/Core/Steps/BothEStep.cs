using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
        public static readonly BothEStep NoLabels = new(ImmutableArray<string>.Empty);

        [Obsolete("Deprecated. Use BothEStep.NoLabels instead.", true)]
        public BothEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public BothEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
