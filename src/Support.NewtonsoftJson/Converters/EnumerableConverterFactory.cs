using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

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
            where TTargetArray : class
        {
            public ArrayConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JArray, TTargetArray>.TryConvert(JArray serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (!Environment.SupportsType(typeof(TTargetArray)))
                {
                    value = Unsafe.As<TTargetArray>(GetEnumerable(serialized, recurse).ToArray());
                    return true;
                }

                value = null;
                return false;
            }
        }

        private sealed class ListConverter<TTarget, TTargetItem> : EnumerableConverter<TTargetItem>, IConverter<JArray, TTarget>
            where TTarget : class
        {
            public ListConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JArray, TTarget>.TryConvert(JArray serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                value = Unsafe.As<TTarget>(GetEnumerable(serialized, recurse).ToList());
                return true;
            }
        }

        private sealed class CollectionConverter<TTarget> : EnumerableConverter<object>, IConverter<JArray, TTarget>
            where TTarget : class
        {
            private readonly ConstructorInfo _constructor;

            public CollectionConverter(ConstructorInfo constructor, IGremlinQueryEnvironment environment) : base(environment)
            {
                _constructor = constructor;
            }

            bool IConverter<JArray, TTarget>.TryConvert(JArray serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                value = (TTarget)_constructor.Invoke([GetEnumerable(serialized, recurse).ToList()]);

                return true;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            if (typeof(TSource) == typeof(JArray))
            {
                if (typeof(TTarget).IsAssignableFrom(typeof(object[])))
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ArrayConverter<,>).MakeGenericType(typeof(TTarget), typeof(object)), environment);

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

                // The non-generic collections - ArrayList, Queue, Stack - are assignable from
                // nothing we build, but they all take an ICollection. Without this they end up at
                // Newtonsoft, which fills them with raw JTokens instead of converted values.
                if (!typeof(TTarget).IsGenericType && typeof(IEnumerable).IsAssignableFrom(typeof(TTarget)) && typeof(TTarget).GetConstructor([typeof(ICollection)]) is { } constructor)
                    return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(CollectionConverter<>).MakeGenericType(typeof(TTarget)), constructor, environment);
            }

            return null;
        }
    }
}
