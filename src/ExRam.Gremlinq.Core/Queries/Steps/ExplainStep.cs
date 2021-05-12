namespace ExRam.Gremlinq.Core
{
    public sealed class ExplainStep : Step
    {
        public static readonly ExplainStep Instance = new();

        public ExplainStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ExplainStep(semantics);
    }
}
