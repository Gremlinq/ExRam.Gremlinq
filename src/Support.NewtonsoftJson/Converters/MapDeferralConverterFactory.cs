using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;
using Gremlin.Net.Process.Traversal;

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

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                    {
                        var retObject = new JObject();
                        var maybeIdToken = default(JToken?);
                        var maybeLabelToken = default(JToken?);

                        for (var i = 0; i < mapArray.Count / 2; i++)
                        {
                            if (mapArray[i * 2].TryParseKey(out var key))
                            {
                                var mapValue = mapArray[i * 2 + 1];

                                if (key.RawKey is string stringKey)
                                    retObject.Add(stringKey, mapValue);
                                else if (key.RawKey is T t)
                                {
                                    if (T.Id.Equals(t))
                                        maybeIdToken = mapValue;
                                    else if (T.Label.Equals(t))
                                        maybeLabelToken = mapValue;
                                }
                            }
                        }

                        if (maybeIdToken is { } idToken && maybeLabelToken is { } labelToken)
                        {
                            retObject = new JObject()
                            {
                                { "id", idToken },
                                { "label", labelToken },
                                { "properties", retObject }
                            };
                        }

                        return recurse.TryTransform(retObject, _environment, out value);
                    }
                }

                value = default;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(TSource) == typeof(JObject)
                ? Unsafe.As<IConverter<TSource, TTarget>>(new MapDeferralConverter<TTarget>(environment))
                : null;
        }
    }
}
