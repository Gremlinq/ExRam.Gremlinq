using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public class VertexSchemaInfo
    {
        public VertexSchemaInfo(VertexTypeInfo typeInfo, ImmutableList<string> indexProperties)
        {
            this.TypeInfo = typeInfo;
            IndexProperties = indexProperties;
        }

        public VertexTypeInfo TypeInfo { get; }
        public ImmutableList<string> IndexProperties { get; }
    }
}