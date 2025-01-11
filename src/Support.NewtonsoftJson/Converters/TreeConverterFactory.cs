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

                value = null;
                return false;
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
