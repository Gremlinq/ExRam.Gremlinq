namespace ExRam.Gremlinq.Core
{
    public sealed class BothVStep : Step
    {
        public static readonly BothVStep Instance = new();

        public BothVStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new BothVStep(semantics);
    }
}
