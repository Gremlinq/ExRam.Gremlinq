using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public class EdgeSchemaInfo
    {
        public EdgeSchemaInfo(string label, ImmutableList<string> properties)
        {
            Label = label;
            Properties = properties;
        }

        public string Label { get; }
        public ImmutableList<string> Properties { get; }
    }
}