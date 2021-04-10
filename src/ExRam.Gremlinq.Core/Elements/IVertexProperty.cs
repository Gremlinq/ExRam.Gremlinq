using System.Collections.Generic;

namespace ExRam.Gremlinq.Core.GraphElements
{
    internal interface IVertexProperty : IProperty
    {
        IEnumerable<KeyValuePair<string, object>> GetProperties(IGremlinQueryEnvironment environment);

        object? Id { get; set; }
    }
}
