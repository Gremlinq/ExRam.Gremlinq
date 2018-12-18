using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq
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
