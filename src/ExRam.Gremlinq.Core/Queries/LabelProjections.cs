using ExRam.Gremlinq.Core.Projections;

namespace ExRam.Gremlinq.Core
{
    public readonly struct LabelProjections
    {
        public LabelProjections(Projection? stepLabelProjection, Projection? sideEffectLabelProjection)
        {
            StepLabelProjection = stepLabelProjection;
            SideEffectLabelProjection = sideEffectLabelProjection;
        }

        public LabelProjections WithStepLabelProjection(Projection stepLabelProjection) => new(stepLabelProjection, SideEffectLabelProjection);
        public LabelProjections WithSideEffectLabelProjection(Projection sideEffectLabelProjection) => new(StepLabelProjection, sideEffectLabelProjection);

        public Projection? StepLabelProjection { get; }
        public Projection? SideEffectLabelProjection { get; }
    }
}
