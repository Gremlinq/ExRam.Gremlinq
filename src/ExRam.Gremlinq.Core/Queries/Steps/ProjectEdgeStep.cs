namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectEdgeStep : Step
    {
        public static readonly ProjectEdgeStep Instance = new();

        public ProjectEdgeStep(QuerySemantics? semantics = null) : base(semantics)
        {

        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics)
        {
            return new ProjectEdgeStep(semantics);
        }
    }
}
