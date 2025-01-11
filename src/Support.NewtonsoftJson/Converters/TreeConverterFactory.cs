using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TreeConverterFactory : IConverterFactory
    {
        private sealed class TreeConverter<TKey> : IConverter<JArray, Tree<TKey>>
            where TKey : notnull
        {
            private readonly IGremlinQueryEnvironment _environment;

            public TreeConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JArray source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out Tree<TKey>? value)
            {
                if (source.Count == 0)
                {
                    value = Tree<TKey>.Empty;
                    return true;
                }

                var dict = new Dictionary<TKey, Tree<object>>();

                foreach (var item in source)
                {
                    if (item is JObject itemObject && itemObject.TryGetValue("key", out var keyToken) && itemObject.TryGetValue("value", out var valueToken))
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        if (recurse.TryTransform(keyToken, _environment, out TKey subKey) && recurse.TryTransform(valueToken, _environment, out Tree<object> subValue))
                        {
                            dict[subKey] = subValue;
                        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                }

                value = new Tree<TKey>(dict);
                return true;
            }
        }

        private sealed class TreeConverter<TKey, TValue> : IConverter<JArray, Tree<TKey, TValue>>
            where TKey : notnull
            where TValue : ITree
        {
            private readonly IGremlinQueryEnvironment _environment;

            public TreeConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JArray source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out Tree<TKey, TValue>? value)
            {
                if (source.Count == 0)
                {
                    value = Tree<TKey, TValue>.Empty;
                    return true;
                }

                if (recurse.TryTransform<JArray, IDictionary<TKey, TValue>>(source, _environment, out var dict))
                {
                    value = new Tree<TKey, TValue>(dict);
                    return true;
                }

                value = null;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(TSource) == typeof(JArray) && typeof(ITree).IsAssignableFrom(typeof(TTarget)))
            {
                if (typeof(TTarget).IsGenericType)
                {
                    var genericArguments = typeof(TTarget).GetGenericArguments();

                    if (typeof(TTarget).GetGenericTypeDefinition() == typeof(Tree<>))
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(TreeConverter<>).MakeGenericType(genericArguments[0]), environment);

                    if (typeof(TTarget).GetGenericTypeDefinition() == typeof(Tree<,>))
                        return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(TreeConverter<,>).MakeGenericType(genericArguments[0], genericArguments[1]), environment);
                }
            }

            return null;
        }
    }
}
