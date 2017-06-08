using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public class VertexSchemaInfo
    {
        public VertexSchemaInfo(string label, ImmutableList<string> properties, ImmutableList<string> partitionKeyProperties, ImmutableList<string> indexProperties)
        {
            Label = label;
            Properties = properties;
            IndexProperties = indexProperties;
            PartitionKeyProperties = partitionKeyProperties;
        }

        public string Label { get; }
        public ImmutableList<string> Properties { get; }
        public ImmutableList<string> IndexProperties { get; }
        public ImmutableList<string> PartitionKeyProperties { get; }
    }
}