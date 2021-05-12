namespace ExRam.Gremlinq.Core
{
    public sealed class ProfileStep : Step
    {
        public static readonly ProfileStep Instance = new();

        public ProfileStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ProfileStep(semantics);
    }
}
