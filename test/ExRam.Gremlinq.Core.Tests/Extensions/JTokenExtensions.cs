using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class JTokenExtensions
    {
        public static JToken Canonicalize(this JToken jToken)
        {
            return jToken switch
            {
                JValue jValue => jValue,
                JObject jObject => jObject.Canonicalize(),
                JArray jArray => jArray.Canonicalize(),
                JProperty jProperty => jProperty.Canonicalize(),
                _ => throw new ArgumentException()
            };
        }

        public static JObject Canonicalize(this JObject jObject)
        {
            if (jObject["@type"]?.ToString() is { } type)
            {
                if (type == "g:Map")
                {
                    if (jObject["@value"] is JArray valueArray)
                    {
                        var newValueArray = new JArray(Enumerable
                            .Range(0, valueArray.Count / 2)
                            .Select(i => (valueArray[i * 2], valueArray[i * 2 + 1]))
                            .Where(x => x.Item1 is JValue {Type: JTokenType.String})
                            .OrderBy(x => x.Item1.ToString())
                            .SelectMany(x => new[] {x.Item1, x.Item2.Canonicalize()}));

                        return new JObject(
                            new JProperty("@type", "g:Map"),
                            new JProperty("@value", newValueArray));
                    }
                }
                //else if (type == "g:Int32" || type == "g:Int64" || type == "g:UUID")
                //{
                //    return new JObject(
                //        new JProperty("@type", type),
                //        new JProperty("@value", "scrubbed id"));
                //}
            }

            return new(
                jObject
                    .Properties()
                    .OrderBy(x => x.Name)
                    .Select(property => property.Canonicalize()));
        }

        public static JArray Canonicalize(this JArray jArray)
        {
            return new(jArray
                .Select(x => x.Canonicalize())
                .OrderBy(x => x.ToString()));
        }

        public static JProperty Canonicalize(this JProperty jProperty)
        {
            return new(jProperty.Name, jProperty.Value.Canonicalize());
        }
    }
}
