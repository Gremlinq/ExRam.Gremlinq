using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
        public static readonly OutEStep NoLabels = new();

        //TODO: Think about making this private to force use of NoLabels.
        public OutEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public OutEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
