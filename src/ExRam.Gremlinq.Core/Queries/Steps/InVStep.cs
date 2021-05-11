namespace ExRam.Gremlinq.Core
{
    public sealed class InVStep : Step
    {
        public static readonly InVStep Instance = new();

        public InVStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }
    }
}
