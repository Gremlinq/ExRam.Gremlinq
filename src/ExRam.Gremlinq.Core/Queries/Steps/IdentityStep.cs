namespace ExRam.Gremlinq.Core
{
    public sealed class IdentityStep : Step
    {
        public static readonly IdentityStep Instance = new();

        public IdentityStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new IdentityStep(semantics);
    }
}
