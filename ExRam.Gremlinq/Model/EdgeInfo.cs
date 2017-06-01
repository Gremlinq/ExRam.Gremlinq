using System;

namespace ExRam.Gremlinq
{
    public sealed class EdgeInfo : GraphElementInfo
    {
        public EdgeInfo(Type elementType, string label) : base(elementType, label)
        {
        }
    }
}