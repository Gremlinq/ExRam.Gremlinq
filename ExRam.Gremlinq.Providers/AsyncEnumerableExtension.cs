using System.Linq;
using ExRam.Gremlinq.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace System.Collections.Generic
{
    public static class AsyncEnumerableExtension
    {
        public static IAsyncEnumerable<TElement> GraphsonDeserialize<TElement>(this IAsyncEnumerable<JToken> tokenEnumerable, JsonSerializer serializer)
        {
            return tokenEnumerable
                .Select(token => serializer
                    .Deserialize<TElement>(new JTokenReader(token)
                        .ToTokenEnumerable()
                        .Apply(JsonTransform
                            .Identity()
                            .GraphElements()
                            .NestedValues())
                        .ToJsonReader()));
        }
    }
}
