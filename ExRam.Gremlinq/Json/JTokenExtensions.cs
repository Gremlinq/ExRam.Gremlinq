using System;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal static class JTokenExtensions
    {
        public static Option<JToken> Transform(this JToken token, JsonTransformRule rule)
        {
            Option<JToken> Recurse(JToken ast) => rule(ast, Recurse);

            return Recurse(token);
        }

        public static bool Has(this JObject jObject, string key, object value)
        {
            var keyValue = (jObject[key] as JValue)?.Value;

            if (keyValue is string stringValue)
                return stringValue.Equals(value as string, StringComparison.OrdinalIgnoreCase);

            return (keyValue?.Equals(value)).GetValueOrDefault();
        }
    }
}
