using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider<out TNative> : IHasModel
    {
        IAsyncEnumerable<TNative> Execute(IGremlinQuery query);
    }
}