using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

namespace Newtonsoft.Json.Linq
{
    internal static class JTokenExtensions
    {
        public static IEnumerable<TItem>? TryExpandTraverser<TItem>(this JObject jObject, IGremlinQueryEnvironment env, ITransformer recurse)
        {
            if (jObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && jObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
            {
                var bulk = 1;

                if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken) && recurse.TryTransform<JToken, int>(bulkToken, env, out var bulkObject))
                    bulk = bulkObject;

                if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                {
                    return Core();

                    IEnumerable<TItem> Core()
                    {
                        if (recurse.TryTransform<JToken, TItem>(traverserValue, env, out var item))
                        {
                            for (var j = 0; j < bulk; j++)
                                yield return item;
                        }
                    }
                }
            }

            return null;
        }

        public static bool LooksLikeProperty(this JObject jObject)
        {
            if (jObject.ContainsKey("value"))
            {
                if (jObject.TryGetValue("key", out var keyToken))
                {
                    if (keyToken.Type != JTokenType.String)
                        return false;

                    return jObject.Count == 2;
                }

                return jObject.Count == 1;
            }

            return false;
        }

        public static bool LooksLikeVertexProperty(this JObject jObject)
        {
            if (jObject.ContainsKey("value") && jObject.TryGetValue("id", out var idToken))
            {
                if (idToken.Type == JTokenType.Array)
                    return false;

                var count = 2;

                if (jObject.TryGetValue("label", out var labelToken))
                {
                    if (labelToken.Type != JTokenType.String)
                        return false;

                    count++;
                }

                if (jObject.TryGetValue("properties", out var propertiesToken))
                {
                    if (propertiesToken.Type != JTokenType.Object)
                        return false;

                    count++;
                }

                return jObject.Count == count;
            }

            return false;
        }
    }
}
