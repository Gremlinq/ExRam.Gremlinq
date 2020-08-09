using System;

namespace ExRam.Gremlinq.Core
{
    internal interface IGraphElementModelCache
    {
        string GetLabel(Type type);
    }
}