using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
        public static readonly OutStep NoLabels = new();

        //TODO: Think about making this private to force use of NoLabels.
        public OutStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public OutStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
