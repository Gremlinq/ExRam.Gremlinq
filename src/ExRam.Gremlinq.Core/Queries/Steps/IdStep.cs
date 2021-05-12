namespace ExRam.Gremlinq.Core
{
    public sealed class IdStep : Step
    {
        public static readonly IdStep Instance = new();

        public IdStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new IdStep(semantics);
    }
}
