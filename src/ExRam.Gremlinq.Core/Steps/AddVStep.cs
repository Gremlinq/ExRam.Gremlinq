namespace ExRam.Gremlinq.Core
{
    public sealed class AddVStep : Step
    {
        public AddVStep(string label) : base()
        {
            Label = label;
        }

        public string Label { get; }
    }
}
