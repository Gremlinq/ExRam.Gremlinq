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

        public static bool LooksLikeProperty(this JObject jObject) =>
            jObject.Count == 2 &&
            jObject.TryGetValue("key", out var keyToken) &&
            keyToken.Type == JTokenType.String &&
            jObject.ContainsKey("value");

        public static bool LooksLikeVertexProperty(this JObject jObject) =>
            jObject.Count <= 4 &&
            jObject.TryGetValue("id", out var idToken) &&
            idToken.Type != JTokenType.Array &&
            jObject.TryGetValue("label", out var labelToken) &&
            labelToken.Type == JTokenType.String &&
            jObject.ContainsKey("value") &&
            (!jObject.TryGetValue("properties", out var propertiesToken) || (propertiesToken.Type == JTokenType.Object && jObject.Count == 4));
    }
}
