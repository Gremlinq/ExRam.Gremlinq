using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public class VertexSchemaInfo
    {
        public VertexSchemaInfo(VertexTypeInfo typeInfo)
        {
            this.TypeInfo = typeInfo;
        }

        public VertexTypeInfo TypeInfo { get; }
    }
}