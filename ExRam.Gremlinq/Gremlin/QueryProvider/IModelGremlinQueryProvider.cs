using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider : IHasModel
    {
        IAsyncEnumerable<string> Execute(IGremlinQuery query);
    }
}