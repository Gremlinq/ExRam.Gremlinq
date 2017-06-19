using System;

namespace ExRam.Gremlinq
{
    public sealed class VertexTypeInfo : GraphElementInfo
    {
        public VertexTypeInfo(Type elementType, string label) : base(elementType, label)
        {
        }
    }
}