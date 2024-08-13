using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
        public static readonly InEStep NoLabels = new();

        //TODO: Think about making this private to force use of NoLabels.
        public InEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public InEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
