using System.Linq;
using LanguageExt;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
{
    internal static class JsonTransformRules
    {
        public static readonly JsonTransformRule Empty;
        public static readonly JsonTransformRule Identity;

        static JsonTransformRules()
        {
            Empty = (token, recurse) => Option<JToken>.None;

            Identity = (token, recurse) =>
            {
                if (token is JArray jArray)
                {
                    return new JArray(jArray
                        .SelectMany(item => recurse(item)
                            .AsEnumerable()));
                }

                if (token is JObject jObject)
                {
                    return new JObject(jObject
                        .Properties()
                        .SelectMany(prop => recurse(prop)
                            .AsEnumerable()));
                }

                if (token is JProperty property)
                {
                    return recurse(property.Value)
                        .Map(transformedPropertyValue => (JToken)new JProperty(property.Name, transformedPropertyValue));
                }

                return token;
            };
        }

        public static JsonTransformRule Lazy(this JsonTransformRule firstRule, JsonTransformRule secondRule) => (token, recurse) =>
        {
            return firstRule(token, recurse).IfNone(() => secondRule(token, recurse));
        };

        public static JsonTransformRule Eager(this JsonTransformRule firstRule, JsonTransformRule secondRule) => (token, recurse) =>
        {
            return firstRule(token, recurse).Bind(first => secondRule(first, recurse));
        };
    }
}