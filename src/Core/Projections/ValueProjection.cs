namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for scalar value results.</summary>
    public sealed class ValueProjection : Projection
    {
        /// <inheritdoc />
        public override Projection Lower() => Empty;
    }
}
