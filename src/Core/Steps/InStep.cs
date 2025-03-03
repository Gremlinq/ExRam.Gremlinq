using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class InStep : DerivedLabelNamesStep
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly InStep NoLabels = new();
#pragma warning restore CS0618 // Type or member is obsolete

        [Obsolete("Deprected. Use InStep.NoLabels instead.")]
        public InStep() : this(ImmutableArray<string>.Empty)
        {
        }

        public InStep(ImmutableArray<string> labels) : base(labels)
        {
        }
    }
}
