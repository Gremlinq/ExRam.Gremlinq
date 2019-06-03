using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal interface IJsonTransform
    {
        IEnumerator<(JsonToken tokenType, object tokenValue)> Transform(IPebbleEnumerator<(JsonToken tokenType, object tokenValue)> enumerator, IJsonTransform recurse);
    }
}
