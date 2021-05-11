namespace ExRam.Gremlinq.Core
{
    public sealed class AddVStep : Step
    {
        public AddVStep(string label, QuerySemantics? semantics = default) : base(semantics)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
