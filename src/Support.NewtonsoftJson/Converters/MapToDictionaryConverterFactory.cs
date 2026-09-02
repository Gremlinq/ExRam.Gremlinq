using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class MapToDictionaryConverterFactory : IConverterFactory
    {
        private abstract class MapConverter<TKey, TValue>
            where TKey : notnull
        {
            private readonly IGremlinQueryEnvironment _environment;

            protected MapConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            protected Dictionary<TKey, TValue>? TryBuild(JObject serialized, ITransformer recurse)
            {
                if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                    {
                        var retObject = new Dictionary<TKey, TValue>();

                        for (var i = 0; i < mapArray.Count / 2; i++)
                        {
                            if (recurse.TryTransform(mapArray[i * 2], _environment, out TKey? key) && recurse.TryTransform(mapArray[i * 2 + 1], _environment, out TValue? entry))
                                retObject.Add(key, entry);
                        }

                        return retObject;
                    }
                }

                return null;
            }
        }

        private sealed class MapToDictionaryConverter<TKey, TValue, TTarget> : MapConverter<TKey, TValue>, IConverter<JObject, TTarget>
            where TKey : notnull
            where TTarget : class
        {
            public MapToDictionaryConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (TryBuild(serialized, recurse) is { } dictionary)
                {
                    value = Unsafe.As<TTarget>(dictionary);

                    return true;
                }

                value = null;

                return false;
            }
        }

        private sealed class MapToImmutableDictionaryConverter<TKey, TValue, TTarget> : MapConverter<TKey, TValue>, IConverter<JObject, TTarget>
            where TKey : notnull
            where TTarget : class
        {
            public MapToImmutableDictionaryConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (TryBuild(serialized, recurse) is { } dictionary)
                {
                    value = Unsafe.As<TTarget>(dictionary.ToImmutableDictionary());

                    return true;
                }

                value = null;

                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            if (typeof(TSource) == typeof(JObject))
            {
                // A g:Map carries its keys as tokens, so it is this converter that has to build the
                // dictionary - the deferral to a JObject can only represent keys that are strings.
                // Hence both concrete types worth building are looked for, not just Dictionary<,>.
                var maybeMatch = typeof(TTarget)
                    .GetInterfaces()
                    .Prepend(typeof(TTarget))
                    .Select(static iface =>
                    {
                        if (iface is { IsGenericType: true, GenericTypeArguments: [var keyType, var valueType] })
                        {
                            if (typeof(TTarget).IsAssignableFrom(typeof(Dictionary<,>).MakeGenericType(keyType, valueType)))
                                return (keyType, valueType, converterType: typeof(MapToDictionaryConverter<,,>));

                            if (typeof(TTarget).IsAssignableFrom(typeof(ImmutableDictionary<,>).MakeGenericType(keyType, valueType)))
                                return (keyType, valueType, converterType: typeof(MapToImmutableDictionaryConverter<,,>));
                        }

                        return default((Type keyType, Type valueType, Type converterType)?);
                    })
                    .FirstOrDefault(static x => x != null);

                if (maybeMatch is ({ } keyType, { } valueType, { } converterType))
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(converterType.MakeGenericType(keyType, valueType, typeof(TTarget)), environment);
            }

            return null;
        }
    }
}
