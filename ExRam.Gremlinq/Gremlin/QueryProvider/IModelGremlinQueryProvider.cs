using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider
    {
        IAsyncEnumerable<string> Execute(IGremlinQuery query);
        
        IGraphModel Model { get; }
    }
}