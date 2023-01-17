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
    }
}
