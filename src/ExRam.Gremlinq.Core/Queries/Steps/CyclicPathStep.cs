namespace ExRam.Gremlinq.Core
{
    public sealed class CyclicPathStep : Step
    {
        public CyclicPathStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new CyclicPathStep(semantics);
    }
}
