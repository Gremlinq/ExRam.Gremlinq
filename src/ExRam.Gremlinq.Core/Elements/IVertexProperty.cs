using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    internal interface IVertexProperty : IProperty, IElement
    {
        IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment);
    }
}
