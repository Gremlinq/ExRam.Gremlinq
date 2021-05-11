namespace ExRam.Gremlinq.Core
{
    public sealed class CyclicPathStep : Step
    {
        public CyclicPathStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }
    }

    public sealed class SimplePathStep : Step
    {
        public SimplePathStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }
    }
}
