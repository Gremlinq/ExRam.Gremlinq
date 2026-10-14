using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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

        private sealed class MapToImmutableSortedDictionaryConverter<TKey, TValue, TTarget> : MapConverter<TKey, TValue>, IConverter<JObject, TTarget>
            where TKey : notnull
            where TTarget : class
        {
            public MapToImmutableSortedDictionaryConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                // A builder, as for an ImmutableDictionary<,>: the type itself has no constructor
                // to make it empty with.
                if (TryBuild(serialized, recurse, static _ => ImmutableSortedDictionary.CreateBuilder<TKey, TValue>()) is { } builder)
                {
                    value = Unsafe.As<TTarget>(builder.ToImmutable());

                    return true;
                }

                value = null;

                return false;
            }
        }

        private sealed class MapToConstructibleDictionaryConverter<TKey, TValue, TTarget> : MapConverter<TKey, TValue>, IConverter<JObject, TTarget>
            where TKey : notnull
            where TTarget : class, IDictionary<TKey, TValue>, new()
        {
            public MapToConstructibleDictionaryConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                // No capacity: the parameterless constructor is the one constructor every such
                // dictionary is known to have.
                value = TryBuild(serialized, recurse, static _ => new TTarget());

                return value is not null;
            }
        }

        private sealed class MapToConstructedDictionaryConverter<TKey, TValue, TTarget> : MapConverter<TKey, TValue>, IConverter<JObject, TTarget>
            where TKey : notnull
            where TTarget : class
        {
            private readonly ConstructorInfo _constructor;

            public MapToConstructedDictionaryConverter(IGremlinQueryEnvironment environment, ConstructorInfo constructor) : base(environment)
            {
                _constructor = constructor;
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                // A Dictionary<TKey, TValue> is filled and handed to the constructor of what is asked
                // for. Whatever that constructor throws is the caller's, and fails the query unwrapped.
                if (TryBuild(serialized, recurse, static capacity => new Dictionary<TKey, TValue>(capacity)) is { } dictionary)
                {
                    value = (TTarget)_constructor.Invoke(BindingFlags.DoNotWrapExceptions, null, [dictionary], null);

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
                // Hence the concrete types worth building are looked for, not just Dictionary<,>:
                // ImmutableDictionary<,> for what it stands in for, and ImmutableSortedDictionary<,>,
                // which neither of the other two stands in for and which cannot be made empty.
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

                            if (typeof(TTarget).IsAssignableFrom(typeof(ImmutableSortedDictionary<,>).MakeGenericType(keyType, valueType)))
                                return (keyType, valueType, converterType: typeof(MapToImmutableSortedDictionaryConverter<,,>));
                        }

                        return default((Type keyType, Type valueType, Type converterType)?);
                    })
                    .FirstOrDefault(static x => x != null);

                if (maybeMatch is ({ } keyType, { } valueType, { } converterType))
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(converterType.MakeGenericType(keyType, valueType, typeof(TTarget)), environment);

                // Where none of them stands in for it, the type itself is built, filled through the
                // first IDictionary<TKey, TValue> that Type.GetInterfaces() lists, as Newtonsoft's
                // contract resolver picks it - which one depends on the requested type alone, though
                // .NET does not document that order. The keys and the values are read as that
                // interface says. Only a type without one is read as the first
                // IReadOnlyDictionary<TKey, TValue> it lists, as the contract resolver falls back to
                // it as well.
                if (typeof(TTarget) is { IsClass: true, IsAbstract: false })
                {
                    var interfaces = typeof(TTarget).GetInterfaces();
                    var dictionaryInterface = interfaces.FirstOrDefault(static type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>));

                    // One that can be made empty is filled through that interface - a
                    // SortedDictionary<,>, say, or a class derived from Dictionary<,>, which inherits
                    // none of its other constructors. An IReadOnlyDictionary<,> cannot be filled, so
                    // a type that is only that is never made empty.
                    if (dictionaryInterface is not null && typeof(TTarget).GetConstructor(Type.EmptyTypes) is not null)
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(MapToConstructibleDictionaryConverter<,,>).MakeGenericType([.. dictionaryInterface.GetGenericArguments(), typeof(TTarget)]), environment);

                    dictionaryInterface ??= interfaces.FirstOrDefault(static type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)) is { } readOnlyDictionaryInterface
                        ? typeof(IDictionary<,>).MakeGenericType(readOnlyDictionaryInterface.GetGenericArguments())
                        : null;

                    // Otherwise it is built where a public constructor takes nothing but a dictionary of
                    // what it holds, as a ReadOnlyDictionary<,> has one: a Dictionary<TKey, TValue> is
                    // filled and passed to it. Which constructor that is, TryGetDictionaryConstructor
                    // says. The constructor is looked for once, here.
                    if (dictionaryInterface is not null && TryGetDictionaryConstructor(typeof(TTarget), dictionaryInterface) is { } dictionaryConstructor)
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(MapToConstructedDictionaryConverter<,,>).MakeGenericType([.. dictionaryInterface.GetGenericArguments(), typeof(TTarget)]), environment, dictionaryConstructor);
                }
            }

            return null;
        }

        // The constructor Newtonsoft's serializer builds such a dictionary through: one whose one
        // parameter an IDictionary<TKey, TValue> can be passed as - not an IReadOnlyDictionary<,>,
        // though a Dictionary<,> could be. Of several, the one that takes exactly
        // IEnumerable<KeyValuePair<TKey, TValue>>, wherever it is declared, otherwise the first.
        // That IDictionary<TKey, TValue> need not be one the type implements: for a type that is
        // only an IReadOnlyDictionary<TKey, TValue>, it is made from that one's arguments.
        private static ConstructorInfo? TryGetDictionaryConstructor(Type type, Type dictionaryInterface)
        {
            var pairsType = typeof(IEnumerable<>).MakeGenericType(typeof(KeyValuePair<,>).MakeGenericType(dictionaryInterface.GetGenericArguments()));
            var constructors = type
                .GetConstructors()
                .Where(constructor => constructor.GetParameters() is [{ } parameter] && parameter.ParameterType.IsAssignableFrom(dictionaryInterface))
                .ToArray();

            return constructors.FirstOrDefault(constructor => constructor.GetParameters()[0].ParameterType == pairsType) ?? constructors.FirstOrDefault();
        }
    }
}
