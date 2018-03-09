using System.Collections.Generic;

namespace ExRam.Gremlinq
{
    public interface IGremlinSerializable
    {
        string Serialize(IParameterCache parameterCache);
    }
}