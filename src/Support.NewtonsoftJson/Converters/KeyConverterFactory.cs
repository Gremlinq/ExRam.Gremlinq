using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class KeyConverterFactory : IConverterFactory
    {
        private sealed class KeyConverter<TSource> : IConverter<TSource, Key>
            where TSource : JToken
        {
            public bool TryConvert(TSource serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out Key value)
            {
                if (serialized is JObject jObject)
                {
                    if (jObject.TryGetValue("@type", out var @type) && "g:T".Equals(@type.Value<string>(), StringComparison.OrdinalIgnoreCase) && jObject.TryGetValue("@value", out var valueToken) && valueToken.Type == JTokenType.String && valueToken.Value<string>() is { } stringValue)
                    {
                        value = new Key(T.GetByValue(stringValue));

                        return true;
                    }
                }
                else if (serialized is JValue { Type: JTokenType.String } stringValue)
                {
                    value = new Key(stringValue.ToString());

                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(TTarget) == typeof(Key)
            ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(KeyConverter<>).MakeGenericType(typeof(TSource)))
            : null;
    }
}
