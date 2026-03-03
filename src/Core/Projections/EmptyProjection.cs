namespace ExRam.Gremlinq.Core.Projections
{
    /// <summary>Represents the absence of a projection.</summary>
    public sealed class EmptyProjection : Projection
    {
        /// <inheritdoc />
        public override Projection Lower() => Empty;
    }
}
