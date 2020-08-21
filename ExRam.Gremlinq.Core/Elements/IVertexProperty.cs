using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    public interface IVertexProperty : IProperty, IElement
    {
        IDictionary<string, object> GetProperties(IGremlinQueryEnvironment environment);
    }
}
