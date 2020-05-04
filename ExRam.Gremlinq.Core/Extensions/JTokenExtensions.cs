using System;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class JTokenExtensions
    {
        //TODO: Unmap must be able to handle non-string keys (dictionaries!)
        public static JObject? TryUnmap(this JObject jObject)
        {
            if (jObject.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
            {
                if (jObject.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                {
                    var retObject = new JObject();

                    for (var i = 0; i < mapArray.Count / 2; i++)
                    {
                        retObject.Add(mapArray[i * 2].Value<string>(), mapArray[i * 2 + 1]);
                    }

                    return retObject;
                }
            }

            return null;
        }
    }
}
