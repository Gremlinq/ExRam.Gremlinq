using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IModelGremlinQueryProvider<out TNative> : IHasModel, IHasTraversalSource
    {
        IAsyncEnumerable<TNative> Execute(IGremlinQuery query);
    }
}