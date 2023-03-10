using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TraverserConverterFactory : IConverterFactory
    {
        private sealed class EnumerableConverter<TTargetItem> : IConverter<JArray, IEnumerable<TTargetItem>>
        {
            public bool TryConvert(JArray source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out IEnumerable<TTargetItem>? value)
            {
                IEnumerable<TTargetItem> GetEnumerable()
                {
                    for (var i = 0; i < source.Count; i++)
                    {
                        if (source[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TTargetItem>(environment, recurse) is { } enumerable)
                        {
                            foreach (var item1 in enumerable)
                                yield return item1;
                        }
                        else if (recurse.TryTransform<JToken, TTargetItem>(source[i], environment, out var item2))
                        {
                            yield return item2;
                        }
                    }
                }

                value = GetEnumerable();
                return true;
            }
        }

        private sealed class ArrayConverter<TTargetArray, TTargetItem> : IConverter<JArray, TTargetArray>
        {
            public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTargetArray)))
                {
                    if (recurse.TryTransform(serialized, environment, out IEnumerable<TTargetItem>? enumerable))
                    {
                        value = (TTargetArray)(object)enumerable.ToArray();
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            if (typeof(TSource) == typeof(JArray))
            {
                if (typeof(TTarget).IsArray)
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ArrayConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!));

                if (typeof(TTarget).IsConstructedGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(EnumerableConverter<>).MakeGenericType(typeof(TTarget).GenericTypeArguments[0]));

            }

            return default;
        }
    }
}
