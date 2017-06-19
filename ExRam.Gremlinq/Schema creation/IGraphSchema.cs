using System.Collections.Immutable;

namespace ExRam.Gremlinq
{
    public interface IGraphSchema
    {
        IGraphModel Model { get; }
    }
}