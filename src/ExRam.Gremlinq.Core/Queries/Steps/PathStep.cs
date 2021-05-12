namespace ExRam.Gremlinq.Core
{
    public sealed class PathStep : Step
    {
        public static readonly PathStep Instance = new();

        public PathStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new PathStep(semantics);
    }
}
