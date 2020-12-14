using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public interface IVertexProperty : IProperty, IElement
    {
        IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment);
    }
}
