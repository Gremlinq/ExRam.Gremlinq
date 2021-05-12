namespace ExRam.Gremlinq.Core
{
    public sealed class LabelStep : Step
    {
        public static readonly LabelStep Instance = new();

        public LabelStep(QuerySemantics? semantics = default) : base(semantics)
        {
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new LabelStep(semantics);
    }
}
