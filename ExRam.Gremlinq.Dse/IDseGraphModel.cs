using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Dse
{
    public interface IDseGraphModel : IGraphModel
    {
        IImmutableList<(Type, Type, Type)> Connections { get; }
    }
}