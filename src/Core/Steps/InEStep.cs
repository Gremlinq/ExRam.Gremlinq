using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public static readonly InEStep NoLabels = new(ImmutableArray<string>.Empty);

        [Obsolete("Deprecated. Use InEStep.NoLabels instead.", true)]
        public InEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public InEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
