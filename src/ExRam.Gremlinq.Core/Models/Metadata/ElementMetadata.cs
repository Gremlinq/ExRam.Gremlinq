namespace ExRam.Gremlinq.Core.Models
{
    public readonly struct ElementMetadata
    {
        public ElementMetadata(string label)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
