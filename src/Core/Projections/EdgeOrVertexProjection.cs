namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents a projection for results that may be edges or vertices.</summary>
    public sealed class EdgeOrVertexProjection : Projection
    {
        /// <inheritdoc />
        public override Projection Lower() => Element;
    }
}
