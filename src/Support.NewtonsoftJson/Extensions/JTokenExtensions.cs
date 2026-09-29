using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
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
                        if (!(traverserValue is JValue { Value: null }))
                        {
                            if (recurse.TryTransform<JToken, TItem>(traverserValue, env, out var item))
                            {
                                for (var j = 0; j < bulk; j++)
                                    yield return item;
                            }
                        }
                        else
                        {
                            for (var j = 0; j < bulk; j++)
                                yield return default!;
                        }
                    }
                }
            }

            return null;
        }

        public static bool TryParseKey(this JToken token, out Key key)
        {
            if (token is JObject jObject)
            {
                if (jObject.TryGetValue("@type", out var @type) && "g:T".Equals(@type.Value<string>(), StringComparison.OrdinalIgnoreCase) && jObject.TryGetValue("@value", out var valueToken) && valueToken.Type == JTokenType.String && valueToken.Value<string>() is { } stringValue)
                {
                    key = new Key(T.GetByValue(stringValue));

                    return true;
                }
            }
            else if (token is JValue { Type: JTokenType.String } stringValue)
            {
                key = new Key(stringValue.ToString());

                return true;
            }

            key = default;
            return false;
        }

        public static bool LooksLikeElement(this JObject jObject, [NotNullWhen(true)] out JToken? idToken, [NotNullWhen(true)] out JValue? labelValue, out JObject? propertiesObject)
        {
            idToken = null;
            labelValue = null;
            propertiesObject = null;

            if (!jObject.TryGetValue("value", out _) && jObject.TryGetValue("id", out idToken) && idToken.Type != JTokenType.Array && jObject.TryGetValue("label", out var labelToken) && labelToken.Type == JTokenType.String)
            {
                if ((labelValue = labelToken as JValue) is not null)
                {
                    if (jObject.TryGetValue("properties", out var propertiesToken))
                    {
                        propertiesObject = propertiesToken as JObject;

                        if (propertiesObject is null)
                            return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool LooksLikeProperty(this JObject jObject) => jObject.TryGetValue("value", out _) && jObject.TryGetValue("key", out var keyToken) && keyToken.Type == JTokenType.String;

        public static bool LooksLikeVertexProperty(this JObject jObject)
        {
            if (jObject.TryGetValue("value", out _) && jObject.TryGetValue("id", out var idToken) && idToken.Type != JTokenType.Array)
            {
                if (!jObject.TryGetValue("label", out var labelToken) || labelToken.Type == JTokenType.String)
                {
                    if (!jObject.TryGetValue("properties", out var propertiesToken) || propertiesToken.Type == JTokenType.Object)
                        return true;
                }
            }

            return false;
        }
    }
}
