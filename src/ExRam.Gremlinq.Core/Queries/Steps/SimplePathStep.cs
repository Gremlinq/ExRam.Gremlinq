namespace ExRam.Gremlinq.Core
{
    public sealed class SimplePathStep : Step
    {
        public SimplePathStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new SimplePathStep(semantics);
    }
}
