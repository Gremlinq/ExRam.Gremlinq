using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class BothStep : DerivedLabelNamesStep
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly BothStep NoLabels = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Deprected. Use BothStep.NoLabels instead.")]
        public BothStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public BothStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
