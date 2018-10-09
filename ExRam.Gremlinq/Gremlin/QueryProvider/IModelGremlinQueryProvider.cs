using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider<out TNative>
    {
        IAsyncEnumerable<TNative> Execute(IGremlinQuery query);
    }
}