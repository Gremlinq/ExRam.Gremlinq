using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        (string queryString, IDictionary<string, object> parameters) Serialize(IParameterCache parameterCache);
    }
}