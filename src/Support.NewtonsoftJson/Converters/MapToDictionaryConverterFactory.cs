using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class MapToDictionaryConverterFactory : IConverterFactory
    {
        private sealed class MapToDictionaryConverter<TKey, TValue, TTarget> : IConverter<JObject, TTarget>
            where TKey : notnull
        {
            private readonly IGremlinQueryEnvironment _environment;

            public MapToDictionaryConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
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

                        value = (TTarget)(object)retObject;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(TSource) == typeof(JObject))
            {
                var maybeCompatibleInterface = typeof(TTarget)
                    .GetInterfaces().Prepend(typeof(TTarget))
                    .Select(iface => iface.IsGenericType && iface.GenericTypeArguments is [var keyType, var valueType] && typeof(TTarget).IsAssignableFrom(typeof(Dictionary<,>).MakeGenericType(keyType, valueType))
                        ? (keyType, valueType)
                        : default((Type keyType, Type valueType)?))
                    .FirstOrDefault(x => x != null);

                if (maybeCompatibleInterface is ({ } keyType, { } valueType))
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(MapToDictionaryConverter<,,>).MakeGenericType(keyType, valueType, typeof(TTarget)), environment);
            }

            return default;
        }
    }
}
