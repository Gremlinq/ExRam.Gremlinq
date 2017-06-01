using System;

namespace ExRam.Gremlinq
{
    public abstract class GraphElementInfo
    {
        protected GraphElementInfo(Type elementType, string label)
        {
            this.ElementType = elementType;
            this.Label = label;
        }

        public Type ElementType { get; }
        public string Label { get; }
    }
}