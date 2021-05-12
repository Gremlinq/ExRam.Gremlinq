namespace ExRam.Gremlinq.Core
{
    public sealed class KeyStep : Step
    {
        public static readonly KeyStep Instance = new();

        public KeyStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new KeyStep(semantics);
    }
}
