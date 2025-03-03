using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OutEStep : DerivedLabelNamesStep
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly OutEStep NoLabels = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Deprecated. Use OutEStep.NoLabels instead.")]
        public OutEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public OutEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
