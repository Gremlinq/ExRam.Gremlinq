namespace ExRam.Gremlinq.Core
{
    public sealed class AddVStep : Step
    {
        public AddVStep(string label, QuerySemantics? semantics = default) : base(semantics)
        {
            Label = label;
        }

        public override Step OverrideQuerySemantics(QuerySemantics semantics) => new AddVStep(Label, semantics);

        public string Label { get; }
    }
}
