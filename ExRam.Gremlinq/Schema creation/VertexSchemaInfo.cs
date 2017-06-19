using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public class VertexSchemaInfo
    {
        public VertexSchemaInfo(VertexTypeInfo typeInfo, ImmutableList<string> partitionKeyProperties, ImmutableList<string> indexProperties)
        {
            this.TypeInfo = typeInfo;
            IndexProperties = indexProperties;
            PartitionKeyProperties = partitionKeyProperties;
        }

        public VertexTypeInfo TypeInfo { get; }
        public ImmutableList<string> IndexProperties { get; }
        public ImmutableList<string> PartitionKeyProperties { get; }
    }
}