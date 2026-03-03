namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for generic element results.</summary>
    public sealed class ElementProjection : Projection
    {
        /// <inheritdoc />
        public override Projection Lower() => Empty;
    }
}
