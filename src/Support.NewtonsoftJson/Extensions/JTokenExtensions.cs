using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

namespace Newtonsoft.Json.Linq
{
    internal static class JTokenExtensions
    {
        public static IEnumerable<TItem>? TryExpandTraverser<TItem>(this JObject jObject, IGremlinQueryEnvironment env, ITransformer recurse)
        {
            if (jObject.TryGetValue("@type", StringComparison.OrdinalIgnoreCase, out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && jObject.TryGetValue("@value", StringComparison.OrdinalIgnoreCase, out var valueToken) && valueToken is JObject nestedTraverserObject)
            {
                var bulk = 1;

                if (nestedTraverserObject.TryGetValue("bulk", StringComparison.OrdinalIgnoreCase, out var bulkToken) && recurse.TryTransform<JToken, int>(bulkToken, env, out var bulkObject))
                    bulk = bulkObject;

                if (nestedTraverserObject.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out var traverserValue))
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

        public static bool LooksLikeElement(this JObject jObject, [NotNullWhen(true)] out JToken? idToken, [NotNullWhen(true)] out JValue? labelValue, out JObject? propertiesObject)
        {
            idToken = null;
            labelValue = null;
            propertiesObject = null;

            if (!jObject.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out _) && jObject.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out idToken) && idToken.Type != JTokenType.Array && jObject.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken) && labelToken.Type == JTokenType.String)
            {
                if ((labelValue = labelToken as JValue) is not null)
                {
                    if (jObject.TryGetValue("properties", StringComparison.OrdinalIgnoreCase, out var propertiesToken))
                    {
                        propertiesObject = propertiesToken as JObject;

                        if (propertiesToken is null)
                            return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool LooksLikeProperty(this JObject jObject)
        {
            return jObject.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out _) && jObject.TryGetValue("key", StringComparison.OrdinalIgnoreCase, out var keyToken) && keyToken.Type == JTokenType.String;
        }

        public static bool LooksLikeVertexProperty(this JObject jObject)
        {
            if (jObject.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out _) && jObject.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out var idToken) && idToken.Type != JTokenType.Array)
            {
                if (!jObject.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken) || labelToken.Type == JTokenType.String)
                {
                    if (!jObject.TryGetValue("properties", StringComparison.OrdinalIgnoreCase, out var propertiesToken) || propertiesToken.Type == JTokenType.Object)
                        return true;
                }
            }

            return false;
        }
    }
}
