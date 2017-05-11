using System;

namespace ExRam.Gremlinq
{
    public struct GraphElementInfo
    {
        public GraphElementInfo(Type elementType, string label)
        {
            this.ElementType = elementType;
            this.Label = label;
        }

        public Type ElementType { get; }
        public string Label { get; }
    }
}