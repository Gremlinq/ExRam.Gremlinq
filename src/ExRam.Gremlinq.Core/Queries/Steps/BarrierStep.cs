namespace ExRam.Gremlinq.Core
{
    public sealed class BarrierStep : Step
    {
        public static readonly BarrierStep Instance = new();

        public BarrierStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new BarrierStep(semantics);
    }
}
