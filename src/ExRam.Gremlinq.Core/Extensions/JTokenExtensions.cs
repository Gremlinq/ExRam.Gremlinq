using System;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class JTokenExtensions
    {
        //TODO: Unmap must be able to handle non-string keys (dictionaries!)
        public static JObject? TryUnmap(this JObject jObject, IGremlinQueryEnvironment env, IGremlinQueryFragmentDeserializer recurse)
        {
            if (jObject.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
            {
                if (jObject.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                {
                    var retObject = new JObject();

                    for (var i = 0; i < mapArray.Count / 2; i++)
                    {
                        retObject.Add(mapArray[i * 2].Value<string>(), (JToken)recurse.TryDeserialize(mapArray[i * 2 + 1], typeof(JToken), env)!);
                    }

                    return retObject;
                }
            }

            return null;
        }

        public static JObject? TryGetElementProperties(this JObject jObject, IGremlinQueryEnvironment env, IGremlinQueryFragmentDeserializer recurse)
        {
            return jObject.ContainsKey("id") && jObject.TryGetValue("label", out var label) && label.Type == JTokenType.String && jObject["properties"] is JObject propertiesToken
                ? recurse.TryDeserialize(propertiesToken, typeof(JObject), env) is JObject unmapped
                    ? unmapped
                    : propertiesToken
                : null;
        }
    }
}
