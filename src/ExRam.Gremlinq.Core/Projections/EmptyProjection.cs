namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class EmptyProjection : Projection
    {
        public override Projection Lower() => Empty;
    }
}
