using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OutStep : DerivedLabelNamesStep
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly OutStep NoLabels = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Deprected. Use OutStep.NoLabels instead.")]
        public OutStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public OutStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
