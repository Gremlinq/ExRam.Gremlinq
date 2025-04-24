using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class InStep : DerivedLabelNamesStep
    {
        public static readonly InStep NoLabels = new(ImmutableArray<string>.Empty);

        [Obsolete("Deprecated. Use InStep.NoLabels instead.", true)]
        public InStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public InStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
