using System;

namespace ExRam.Gremlinq
{
    public interface IGraphElementNamingStrategy
    {
        string GetLabelForType(Type type);
    }
}