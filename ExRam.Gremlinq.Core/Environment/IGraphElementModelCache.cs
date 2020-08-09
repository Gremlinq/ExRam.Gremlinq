using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal interface IGraphElementModelCache
    {
        string GetLabel(Type type);
        ImmutableArray<string> GetDerivedLabels(Type type);
    }
}
