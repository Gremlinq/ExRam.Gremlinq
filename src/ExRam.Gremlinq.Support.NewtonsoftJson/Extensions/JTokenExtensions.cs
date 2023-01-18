using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core;

namespace Newtonsoft.Json.Linq
{
    internal static class JTokenExtensions
    {
        public static JObject? TryUnmap(this JObject jObject)
        {
            if (jObject.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                if (jObject.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                {
                    var retObject = new JObject();

                    for (var i = 0; i < mapArray.Count / 2; i++)
                        if (mapArray[i * 2] is JValue { Type: JTokenType.String } key)
                            retObject.Add(key.Value<string>()!, mapArray[i * 2 + 1]);

                    return retObject;
                }

            return null;
        }

        public static IEnumerable<object>? TryExpandTraverser(this JObject jObject, Type type, IGremlinQueryEnvironment env, IGremlinQueryFragmentDeserializer recurse)
        {
            //Traversers
            if (jObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && jObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
            {
                var bulk = 1;

                if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken) && recurse.TryDeserialize(bulkToken, typeof(int), env) is int bulkObject)
                    bulk = bulkObject;

                if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                {
                    return Core();

                    IEnumerable<object> Core()
                    {
                        if (recurse.TryDeserialize(traverserValue, type, env) is { } item)
                        {
                            for (var j = 0; j < bulk; j++)
                                yield return item;
                        }
                    }
                }
            }

            return null;
        }
    }
}
