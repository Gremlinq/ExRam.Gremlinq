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

            private static readonly bool ValueCanBeNull = !typeof(TValue).IsValueType || Nullable.GetUnderlyingType(typeof(TValue)) is not null;

            protected MapConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            // Which dictionary gets filled is the caller's business - deciding that here is what
            // made the immutable converter pay for a Dictionary<,> it then threw away. The g:Map
            // check stays eager: a converter that only discovered it isn't looking at a map
            // partway through would have claimed the conversion already, and every other JObject
            // would come back an empty dictionary instead of reaching the converters behind this one.
            protected TDictionary? TryBuild<TDictionary>(JObject serialized, ITransformer recurse, Func<int, TDictionary> create)
                where TDictionary : class, IDictionary<TKey, TValue>
            {
                if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                {
                    if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                    {
                        // An upper bound - an entry whose key or value doesn't convert is skipped -
                        // which is all a dictionary needs to size itself.
                        var dictionary = create(mapArray.Count / 2);

                        // Set rather than added: a key that is there twice is taken from its last
                        // occurrence instead of failing the whole map. The entry keeps the place of
                        // the first and gets the value of the last - and when that value cannot be
                        // read, there is no entry: the earlier one is not what the map says. The
                        // keys are compared as what they are read as, so a g:Int32 1 and a g:Int64 1
                        // are the same key when longs are asked for.
                        //
                        // A null value is a value where the dictionary's values can hold a null, as
                        // it is for a plain object, and the entry is kept with a null - never with
                        // the JValue that holds it. TryTransform cannot answer a null, so it is the
                        // dictionary that decides this, before anything reads the value, as an array
                        // does for its items.
                        for (var i = 0; i < mapArray.Count / 2; i++)
                        {
                            if (recurse.TryTransform(mapArray[i * 2], _environment, out TKey? key))
                            {
                                var entryToken = mapArray[i * 2 + 1];

                                if (ValueCanBeNull && entryToken.IsNullValue())
                                    dictionary[key] = default!;
                                else if (recurse.TryTransform(entryToken, _environment, out TValue? entry))
                                    dictionary[key] = entry;
                                else
                                    dictionary.Remove(key);
                            }
                        }

                        return dictionary;
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

                if (TryBuild(serialized, recurse, static capacity => new Dictionary<TKey, TValue>(capacity)) is { } dictionary)
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

                // A builder rather than a Dictionary<,> handed to ToImmutableDictionary: the bulk
                // add behind that overload is the same one the builder performs, so the dictionary
                // in between was pure waste.
                if (TryBuild(serialized, recurse, static _ => ImmutableDictionary.CreateBuilder<TKey, TValue>()) is { } builder)
                {
                    value = Unsafe.As<TTarget>(builder.ToImmutable());

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
