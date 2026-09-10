using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Collections;
using System.Collections.Immutable;
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

            // Null when the token is neither of the two things a collection can be built from, so
            // that every converter below declines rather than answering an empty collection - a
            // JObject that is not a bulk set has to reach the converters behind these.
            protected IEnumerable<TTargetItem>? TryGetEnumerable(JToken source, ITransformer recurse)
            {
                if (source is JArray array)
                    return FromArray(array, Environment, recurse);

                // A bulk set is the third way a value arrives, after a plain element and a traverser,
                // and it is read here - where every collection shape is built - rather than in a
                // converter of its own that could only ever build an array.
                if (source is JObject bulkSet
                    && bulkSet.TryGetValue("@type", out var typeToken)
                    && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase)
                    && bulkSet.TryGetValue("@value", out var valueToken)
                    && valueToken is JArray setArray)
                {
                    return FromBulkSet(setArray, Environment, recurse);
                }

                return null;

                static IEnumerable<TTargetItem> FromArray(JArray source, IGremlinQueryEnvironment environment, ITransformer recurse)
                {
                    for (var i = 0; i < source.Count; i++)
                    {
                        if (source[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TTargetItem>(environment, recurse) is { } enumerable)
                        {
                            foreach (var item1 in enumerable)
                                yield return item1;
                        }
                        // A null element is a null item, not an absent one, and this is the only place
                        // that can say so: TryTransform reports a conversion to null as a failure, which
                        // is indistinguishable from a converter declining. It is what a traverser
                        // wrapping null has always yielded - a plain null just never got there.
                        else if (source[i].Type == JTokenType.Null)
                        {
                            yield return default!;
                        }
                        else if (recurse.TryTransform<JToken, TTargetItem>(source[i], environment, out var item2))
                        {
                            yield return item2;
                        }
                    }
                }

                // A bulk set is read in pairs, so an odd number of entries leaves a last element
                // with no bulk to go with it. The loop stops short of it: reading that missing bulk
                // would run off the end of the array and throw - the one outcome a converter must
                // not have - where the well formed prefix is an answer.
                static IEnumerable<TTargetItem> FromBulkSet(JArray setArray, IGremlinQueryEnvironment environment, ITransformer recurse)
                {
                    for (var i = 0; i < setArray.Count - 1; i += 2)
                    {
                        var element = default(TTargetItem)!;

                        // The bulk is read in the same condition as the element because the two
                        // arrive as a pair and neither half means anything without the other. A
                        // bulk nothing can read is not a bulk of one, it is an unknown count, and
                        // an item repeated an unknown number of times cannot be repeated at all.
                        if ((setArray[i].Type == JTokenType.Null || recurse.TryTransform(setArray[i], environment, out element)) && recurse.TryTransform<JToken, int>(setArray[i + 1], environment, out var bulk))
                        {
                            for (var j = 0; j < bulk; j++)
                                yield return element;
                        }
                    }
                }
            }

            protected IGremlinQueryEnvironment Environment { get; }
        }

        private sealed class ArrayConverter<TTargetArray, TTargetItem> : EnumerableConverter<TTargetItem>, IConverter<JToken, TTargetArray>
            where TTargetArray : class
        {
            public ArrayConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JToken, TTargetArray>.TryConvert(JToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (!Environment.SupportsType(typeof(TTargetArray)) && TryGetEnumerable(serialized, recurse) is { } enumerable)
                {
                    value = Unsafe.As<TTargetArray>(enumerable.ToArray());
                    return true;
                }

                value = null;
                return false;
            }
        }

        private sealed class ListConverter<TTarget, TTargetItem> : EnumerableConverter<TTargetItem>, IConverter<JToken, TTarget>
            where TTarget : class
        {
            public ListConverter(IGremlinQueryEnvironment environment) : base(environment)
            {
            }

            bool IConverter<JToken, TTarget>.TryConvert(JToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (TryGetEnumerable(serialized, recurse) is { } enumerable)
                {
                    value = Unsafe.As<TTarget>(enumerable.ToList());
                    return true;
                }

                value = null;
                return false;
            }
        }

        private sealed class CollectionConverter<TTarget> : EnumerableConverter<object>, IConverter<JToken, TTarget>
            where TTarget : class
        {
            private readonly ConstructorInfo _constructor;

            public CollectionConverter(ConstructorInfo constructor, IGremlinQueryEnvironment environment) : base(environment)
            {
                _constructor = constructor;
            }

            bool IConverter<JToken, TTarget>.TryConvert(JToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (TryGetEnumerable(serialized, recurse) is { } enumerable)
                {
                    value = (TTarget)_constructor.Invoke([enumerable.ToList()]);

                    return true;
                }

                value = default;

                return false;
            }
        }

        // The collections List<> cannot stand in for. They are built from the very same item
        // stream, lazily, so a traverser is still converted once and its value yielded as often as
        // its bulk says - no expanded array in between.
        private sealed class SequenceConverter<TTarget, TTargetItem> : EnumerableConverter<TTargetItem>, IConverter<JToken, TTarget>
        {
            private readonly Func<IEnumerable<TTargetItem>, TTarget> _create;

            public SequenceConverter(MethodBase factory, IGremlinQueryEnvironment environment) : base(environment)
            {
                // Once per converter, and converters are cached per requested type.
                _create = factory is ConstructorInfo constructor
                    ? items => (TTarget)constructor.Invoke([items])
                    : ((MethodInfo)factory).CreateDelegate<Func<IEnumerable<TTargetItem>, TTarget>>();
            }

            bool IConverter<JToken, TTarget>.TryConvert(JToken serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(serialized);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                value = TryGetEnumerable(serialized, recurse) is { } enumerable
                    ? _create(enumerable)
                    : default;

                return value is not null;
            }
        }

        // One concrete type per shape, in the order they have to be tried in: IImmutableList<T> is
        // assignable from ImmutableArray<T> as well, and IImmutableSet<T> from ImmutableSortedSet<T>,
        // so the usual implementation of each has to come first.
        private static readonly (Type Definition, MethodInfo CreateRange)[] ImmutableSequences =
        [
            (typeof(ImmutableList<>), CreateRangeMethodOf(typeof(ImmutableList))),
            (typeof(ImmutableHashSet<>), CreateRangeMethodOf(typeof(ImmutableHashSet))),
            (typeof(ImmutableSortedSet<>), CreateRangeMethodOf(typeof(ImmutableSortedSet))),
            (typeof(ImmutableQueue<>), CreateRangeMethodOf(typeof(ImmutableQueue))),
            (typeof(ImmutableStack<>), CreateRangeMethodOf(typeof(ImmutableStack))),
            (typeof(ImmutableArray<>), CreateRangeMethodOf(typeof(ImmutableArray)))
        ];

        // The CreateRange<T>(IEnumerable<T>) of a static factory class, told apart from the
        // overloads taking a comparer, a span or a selector by its shape alone.
        private static MethodInfo CreateRangeMethodOf(Type factoryType) => factoryType
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(static method => method is { Name: nameof(ImmutableList.CreateRange), IsGenericMethodDefinition: true }
                && method.GetGenericArguments() is [var itemType]
                && method.GetParameters() is [var parameter]
                && parameter.ParameterType == typeof(IEnumerable<>).MakeGenericType(itemType));

        private static MethodBase? TryGetSequenceFactory(Type targetType, Type elementType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);

            // The mutable set first: ISet<T> is assignable from ImmutableHashSet<T> too, and asking
            // for one should not be answered with a set that throws on Add.
            if (targetType.IsAssignableFrom(typeof(HashSet<>).MakeGenericType(elementType)))
                return typeof(HashSet<>).MakeGenericType(elementType).GetConstructor([enumerableType]);

            foreach (var (definition, createRange) in ImmutableSequences)
            {
                if (targetType.IsAssignableFrom(definition.MakeGenericType(elementType)))
                    return createRange.MakeGenericMethod(elementType);
            }

            // Queue<T>, Stack<T>, SortedSet<T>, LinkedList<T> and the concurrent collections, none
            // of which needs naming - they all take their items as an IEnumerable<T>.
            return targetType.GetConstructor([enumerableType]);
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            // JToken, not JArray: a bulk set arrives as a JObject, and the collections have to be
            // buildable from it too. Every converter below declines a JObject that is not one.
            if (typeof(JToken).IsAssignableFrom(typeof(TSource)))
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

                        // Everything List<> cannot stand in for - the immutables, the sets, the
                        // queues and stacks. Without this they end up at Newtonsoft, which builds
                        // them but expands no traverser, so a bulk of 7 came back as one item or,
                        // the traverser converting to nothing an int collection will take, threw.
                        if (TryGetSequenceFactory(typeof(TTarget), elementType) is { } factory)
                            return (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(SequenceConverter<,>).MakeGenericType(typeof(TTarget), elementType), factory, environment);
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
