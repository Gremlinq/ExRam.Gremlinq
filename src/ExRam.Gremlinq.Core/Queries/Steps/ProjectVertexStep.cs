namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectVertexStep : Step
    {
        public ProjectVertexStep(QuerySemantics? semantics = null) : base(semantics)
        {

        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics)
        {
            return new ProjectVertexStep(semantics);
        }
    }
}
