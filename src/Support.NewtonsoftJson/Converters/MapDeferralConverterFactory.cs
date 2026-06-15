using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

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

            public bool TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                    {
                        var retObject = new JObject();

                        for (var i = 0; i < mapArray.Count / 2; i++)
                        {
                            if (mapArray[i * 2].TryParseKey(out var key) && key.RawKey is string stringKey)
                                retObject.Add(stringKey, mapArray[i * 2 + 1]);
                        }

                        return recurse.TryTransform(retObject, _environment, out value);
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(JObject)
            ? Unsafe.As<IConverter<TSource, TTarget>>(new MapDeferralConverter<TTarget>(environment))
            : null;
    }
}
