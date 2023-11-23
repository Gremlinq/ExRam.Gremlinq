using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct LabelProjections : IEquatable<LabelProjections>
    {
        public LabelProjections(Projection? stepLabelProjection, Projection? sideEffectLabelProjection)
        {
            StepLabelProjection = stepLabelProjection;
            SideEffectLabelProjection = sideEffectLabelProjection;
        }

        public LabelProjections WithStepLabelProjection(Projection stepLabelProjection) => new(stepLabelProjection, SideEffectLabelProjection);
        public LabelProjections WithSideEffectLabelProjection(Projection sideEffectLabelProjection) => new(StepLabelProjection, sideEffectLabelProjection);

        public bool Equals(LabelProjections other) => StepLabelProjection == other.StepLabelProjection && SideEffectLabelProjection == other.SideEffectLabelProjection;

        public Projection? StepLabelProjection { get; }
        public Projection? SideEffectLabelProjection { get; }
    }
}
