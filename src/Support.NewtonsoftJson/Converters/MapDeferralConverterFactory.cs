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
                        // A JObject can only hold keys that are names, so a map with any other key
                        // would lose those entries on its way through one. Asked for as an object,
                        // such a map is built as the dictionary it is, keys as they were typed.
                        if (typeof(TTarget) == typeof(object))
                        {
                            for (var i = 0; i < mapArray.Count / 2; i++)
                            {
                                if (!mapArray[i * 2].TryParseKey(out _))
                                {
                                    // Whatever MapToDictionaryConverter answers is the answer. It looks for
                                    // the same g:Map this converter has just found, so it has no reason to
                                    // decline - and the JObject road would only lose what this keeps.
                                    var isDictionary = recurse.TryTransform(serialized, _environment, out Dictionary<object, object>? dictionary);

                                    value = (TTarget?)(object?)dictionary;

                                    return isDictionary;
                                }
                            }
                        }

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
