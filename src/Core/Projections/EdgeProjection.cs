namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for edge results.</summary>
    public sealed class EdgeProjection : Projection
    {
        /// <inheritdoc />
        public override Traversal ToTraversal(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return environment.Options.GetValue(GremlinqOption.EdgeProjectionSteps);
        }

        /// <inheritdoc />
        public override Projection Lower() => EdgeOrVertex;
    }
}
