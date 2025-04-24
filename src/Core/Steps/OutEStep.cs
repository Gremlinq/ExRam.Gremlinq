using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public static readonly OutEStep NoLabels = new(ImmutableArray<string>.Empty);

        [Obsolete("Deprecated. Use OutEStep.NoLabels instead.", true)]
        public OutEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public OutEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
