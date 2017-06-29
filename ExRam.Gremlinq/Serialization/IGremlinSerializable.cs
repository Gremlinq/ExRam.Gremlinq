using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        (string queryString, IDictionary<string, object> parameters) Serialize(IGraphModel graphModel, IParameterCache parameterCache, bool inlineParameters);
    }
}