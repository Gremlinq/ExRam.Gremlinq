namespace ExRam.Gremlinq.Core
{
    public sealed class ValueStep : Step
    {
        public static readonly ValueStep Instance = new();

        public ValueStep(QuerySemantics? semantics = default) : base(semantics)
        {

        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new ValueStep(semantics);
    }
}
