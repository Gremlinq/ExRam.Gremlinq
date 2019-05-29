namespace ExRam.Gremlinq.Core
{
    public struct ElementMetadata
    {
        public ElementMetadata(string label)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
