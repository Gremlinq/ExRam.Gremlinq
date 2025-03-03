using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class BothEStep : DerivedLabelNamesStep
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly BothEStep NoLabels = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Deprected. Use BothEStep.NoLabels instead.")]
        public BothEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public BothEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
