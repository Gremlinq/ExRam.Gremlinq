using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TreeConverterFactory : IConverterFactory
    {
        private abstract class TreeConverterBase<TTree, TKey, TValue> : IConverter<JToken, TTree>
            where TTree : ITree
            where TKey : notnull
        {
            private readonly IGremlinQueryEnvironment _environment;

            protected TreeConverterBase(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<JToken, TTree>.TryConvert(JToken source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTree? value)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (source is JArray { Count: 0 } || source is JObject { Count: 0 })
                    value = Create(ImmutableDictionary<TKey, TValue>.Empty);
                else
                {
                    var dictionary = new Dictionary<TKey, TValue>();

                    foreach (var item in source)
                    {
                        var maybeItemObject = (item as JObject) ?? (item as JProperty)?.Value as JObject;

                        if (maybeItemObject is { } itemObject && itemObject.TryGetValue("key", StringComparison.OrdinalIgnoreCase, out var keyToken) && itemObject.TryGetValue("value", StringComparison.OrdinalIgnoreCase, out var valueToken))
                        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                            if (recurse.TryTransform(keyToken, _environment, out TKey subKey) && recurse.TryTransform(valueToken, _environment, out TValue subValue))
                                dictionary[subKey] = subValue;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                        }
                    }

                    value = Create(dictionary);
                }

                return true;
            }

            protected abstract TTree Create(IReadOnlyDictionary<TKey, TValue> dictionary);
        }

        private sealed class TreeConverter<TKey> : TreeConverterBase<Tree<TKey>, TKey, Tree<TKey>>
            where TKey : notnull
        {
            public TreeConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            protected override Tree<TKey> Create(IReadOnlyDictionary<TKey, Tree<TKey>> dictionary) => dictionary.Count == 0
                ? Tree<TKey>.Empty
                : new (dictionary);
        }

        private sealed class TreeConverter<TKey, TValue> : TreeConverterBase<Tree<TKey, TValue>, TKey, TValue>
            where TKey : notnull
            where TValue : ITree
        {
            public TreeConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            protected override Tree<TKey, TValue> Create(IReadOnlyDictionary<TKey, TValue> dictionary) => dictionary.Count == 0
                ? Tree<TKey, TValue>.Empty
                : new (dictionary);
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            if (typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(ITree).IsAssignableFrom(typeof(TTarget)))
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
