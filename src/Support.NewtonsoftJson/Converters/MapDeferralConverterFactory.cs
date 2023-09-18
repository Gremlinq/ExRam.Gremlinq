using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class MapDeferralConverterFactory : IConverterFactory
    {
        private sealed class MapDeferralConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public MapDeferralConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                    {
                        var retObject = new JObject();

                        for (var i = 0; i < mapArray.Count / 2; i++)
                        {
                            if (mapArray[i * 2] is JValue { Type: JTokenType.String } key)
                                retObject.Add(key.Value<string>()!, mapArray[i * 2 + 1]);
                        }

                        return recurse.TryTransform(retObject, _environment, out value);
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject)
                ? (IConverter<TSource, TTarget>)(object)new MapDeferralConverter<TTarget>(environment)
                : default;
        }
    }
}
