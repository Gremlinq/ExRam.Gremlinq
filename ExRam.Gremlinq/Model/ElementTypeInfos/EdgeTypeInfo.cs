using System;

namespace ExRam.Gremlinq
{
    public sealed class EdgeTypeInfo : GraphElementInfo
    {
        public EdgeTypeInfo(Type elementType, string label) : base(elementType, label)
        {
        }
    }
}