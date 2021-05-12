namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectVertexStep : Step
    {
        public static readonly ProjectVertexStep Instance = new();

        public ProjectVertexStep(QuerySemantics? semantics = null) : base(semantics)
        {

        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics)
        {
            return new ProjectVertexStep(semantics);
        }
    }
}
