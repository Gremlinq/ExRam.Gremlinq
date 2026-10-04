using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    // An IEnumerable, an ICollection or an IReadOnlyCollection of KeyValuePair<TKey, TValue> is what a
    // Dictionary<TKey, TValue> is, and it is read as one: whatever reads a Dictionary<TKey, TValue>
    // reads these, with the same answer and the same declines. Left to themselves they were a list
    // of pairs to the collection converters, and a dictionary of another shape than this one to the
    // converters for maps, objects and elements, which only know the generic dictionary types.
    internal sealed class KeyValuePairCollectionConverterFactory : IConverterFactory
    {
        private sealed class KeyValuePairCollectionConverter<TSource, TTarget, TKey, TValue> : IConverter<TSource, TTarget>
            where TSource : JToken
            where TTarget : class
            where TKey : notnull
        {
            private readonly IGremlinQueryEnvironment _environment;

            public KeyValuePairCollectionConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<TSource, TTarget>.TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (recurse.TryTransform(source, _environment, out Dictionary<TKey, TValue>? dictionary))
                {
                    value = Unsafe.As<TTarget>(dictionary);
                    return true;
                }

                value = null;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && IsKeyValuePairCollection(typeof(TTarget), out var keyType, out var valueType)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(KeyValuePairCollectionConverter<,,,>).MakeGenericType(typeof(TSource), typeof(TTarget), keyType, valueType), environment)
                : null;
        }

        // The interfaces of a Dictionary<TKey, TValue> whose single type argument is its item type.
        // IDictionary<TKey, TValue> and IReadOnlyDictionary<TKey, TValue> take their two type
        // arguments as they are, and are read as dictionaries already.
        public static bool IsKeyValuePairCollection(Type type, [NotNullWhen(true)] out Type? keyType, [NotNullWhen(true)] out Type? valueType)
        {
            if (type is { IsInterface: true, IsConstructedGenericType: true, GenericTypeArguments: [{ IsConstructedGenericType: true } itemType] } && itemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>) && itemType.GenericTypeArguments is [var itemKeyType, var itemValueType] && type.IsAssignableFrom(typeof(Dictionary<,>).MakeGenericType(itemKeyType, itemValueType)))
            {
                keyType = itemKeyType;
                valueType = itemValueType;

                return true;
            }

            keyType = null;
            valueType = null;

            return false;
        }
    }
}
