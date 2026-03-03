namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for vertex property results.</summary>
    public sealed class VertexPropertyProjection : Projection
    {
        /// <inheritdoc />
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return environment.Options.GetValue(environment.FeatureSet.Supports(VertexFeatures.MetaProperties)
                ? GremlinqOption.VertexPropertyProjectionSteps
                : GremlinqOption.VertexPropertyProjectionWithoutMetaPropertiesSteps);
        }

        /// <inheritdoc />
        public override Projection Lower() => Element;
    }
}
