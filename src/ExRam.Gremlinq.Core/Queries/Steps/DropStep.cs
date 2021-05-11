namespace ExRam.Gremlinq.Core
{
    public sealed class DropStep : Step
    {
        public static readonly DropStep Instance = new();

        public DropStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }
    }
}
