namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class VertexPropertyProjection : ElementProjection
    {
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment) => environment.Options.GetValue(environment.FeatureSet.Supports(VertexFeatures.MetaProperties)
            ? GremlinqOption.VertexPropertyProjectionSteps
            : GremlinqOption.VertexPropertyProjectionWithoutMetaPropertiesSteps);

        public override Projection Lower() => Element;
    }
}
