namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for vertex results.</summary>
    public sealed class VertexProjection : Projection
    {
        /// <inheritdoc />
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return environment.Options.GetValue(environment.FeatureSet.Supports(VertexFeatures.MetaProperties)
                ? GremlinqOption.VertexProjectionSteps
                : GremlinqOption.VertexProjectionWithoutMetaPropertiesSteps);
        }

        /// <inheritdoc />
        public override Projection Lower() => EdgeOrVertex;
    }
}
