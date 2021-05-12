namespace ExRam.Gremlinq.Core
{
    public sealed class FoldStep : Step
    {
        public static readonly FoldStep Instance = new();

        public FoldStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new FoldStep(semantics);
    }
}
