using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Collections;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class EnumerableConverterFactory : IConverterFactory
    {
        private abstract class EnumerableConverter<TTargetItem>
        {
            protected EnumerableConverter(IGremlinQueryEnvironment environment)
            {
                Environment = environment;
            }

            protected IEnumerable<TTargetItem> GetEnumerable(JArray source, ITransformer recurse)
            {
                for (var i = 0; i < source.Count; i++)
                {
                    if (source[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TTargetItem>(Environment, recurse) is { } enumerable)
                    {
                        foreach (var item1 in enumerable)
                            yield return item1;
                    }
                    else if (recurse.TryTransform<JToken, TTargetItem>(source[i], Environment, out var item2))
                    {
                        yield return item2;
                    }
                }
            }

            protected IGremlinQueryEnvironment Environment { get; }
        }

        private sealed class ArrayConverter<TTargetArray, TTargetItem> : EnumerableConverter<TTargetItem>, IConverter<JArray, TTargetArray>
        {
            public ArrayConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            public bool TryConvert(JArray serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (!Environment.SupportsType(typeof(TTargetArray)))
                {
                    value = (TTargetArray)(object)GetEnumerable(serialized, recurse).ToArray();
                    return true;
                }

                value = default;
                return false;
            }
        }

        private sealed class ListConverter<TTarget, TTargetItem> : EnumerableConverter<TTargetItem>, IConverter<JArray, TTarget>
        {
            public ListConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            public bool TryConvert(JArray serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                value = (TTarget)(object)GetEnumerable(serialized, recurse).ToList();
                return true;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(TSource) == typeof(JArray))
            {
                if (typeof(TTarget).IsAssignableFrom(typeof(object[])))
                    return (IConverter<TSource, TTarget>?)(object)new ArrayConverter<TTarget, object>(environment);

                if (typeof(TTarget).IsArray)
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ArrayConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!), environment);

                if (typeof(TTarget).IsConstructedGenericType && typeof(IEnumerable).IsAssignableFrom(typeof(TTarget)))
                {
                    if (typeof(TTarget).GenericTypeArguments is [ var elementType ])
                    {
                        var listType = typeof(List<>).MakeGenericType(elementType);

                        if (typeof(TTarget).IsAssignableFrom(listType))
                            return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ListConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GenericTypeArguments[0]), environment);
                    }
                }
            }

            return default;
        }
    }
}
