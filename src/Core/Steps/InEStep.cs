using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class InEStep : DerivedLabelNamesStep
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly InEStep NoLabels = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Deprecated. Use InEStep.NoLabels instead.")]
        public InEStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public InEStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
