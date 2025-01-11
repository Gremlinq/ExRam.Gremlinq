using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TreeConverterFactory : IConverterFactory
    {
        private abstract class TreeConverterBase<TTree, TKey, TValue> : IConverter<JArray, TTree>
            where TKey : notnull
        {
            private readonly IGremlinQueryEnvironment _environment;

            public TreeConverterBase(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JArray source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTree? value)
            {
                if (source.Count == 0)
                {
                    value = Create(ImmutableDictionary<TKey, TValue>.Empty)!;
                    return true;
                }

                var dict = new Dictionary<TKey, TValue>();

                foreach (var item in source)
                {
                    if (item is JObject itemObject && itemObject.TryGetValue("key", out var keyToken) && itemObject.TryGetValue("value", out var valueToken))
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        if (recurse.TryTransform(keyToken, _environment, out TKey subKey) && recurse.TryTransform(valueToken, _environment, out TValue subValue))
                        {
                            dict[subKey] = subValue;
                        }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                }

                value = Create(dict)!;
                return true;
            }

            protected abstract TTree Create(IDictionary<TKey, TValue> dictionary);
        }

        private sealed class TreeConverter<TKey> : TreeConverterBase<Tree<TKey>, TKey, Tree<object>>, IConverter<JArray, Tree<TKey>>
            where TKey : notnull
        {
            public TreeConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            protected override Tree<TKey> Create(IDictionary<TKey, Tree<object>> dictionary) => new (dictionary);
        }

        private sealed class TreeConverter<TKey, TValue> : TreeConverterBase<Tree<TKey, TValue>, TKey, TValue>, IConverter<JArray, Tree<TKey, TValue>>
            where TKey : notnull
            where TValue : ITree
        {
            public TreeConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            protected override Tree<TKey, TValue> Create(IDictionary<TKey, TValue> dictionary) => new (dictionary);
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
