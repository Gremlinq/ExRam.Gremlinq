namespace ExRam.Gremlinq.Core.Projections
{
    public sealed class ValueProjection : Projection
    {
        public override Projection Lower() => Empty;
    }
}
