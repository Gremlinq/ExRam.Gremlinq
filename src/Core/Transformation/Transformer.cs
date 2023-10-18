using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Core.Transformation
{
    public static class Transformer
    {
        private sealed class InvalidTransformer : ITransformer
        {
            public ITransformer Add(IConverterFactory converterFactory) => Empty.Add(converterFactory);

            public bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value) => throw new InvalidOperationException("""
                No deserializer configured!
                In Gremlinq v12, query result deserialization has been decoupled from the core library.
                To keep using Newtonsoft.Json as Json-deserialization mechanism, add a reference to
                ExRam.Gremlinq.Support.NewtonsoftJson (or ExRam.Gremlinq.Support.NewtonsoftJson.AspNet on ASP.NET Core)
                and call 'UseNewtonsoftJson()' in the provider configuration.
               
                Examples:
               
                Provider configuration
               
                    IGremlinQuerySource g = ...
               
                    g = g.UseCosmosDb(c => c
                            .UseNewtonsoftJson());
               
                ASP.NET Core
               
                    IServiceCollection services = ...
               
                    services.AddGremlinq(setup => setup
                        .UseCosmosDb(providerSetup => providerSetup
                            .UseNewtonsoftJson()));
               
                Manual configuration
               
                    IGremlinQuerySource g = ...
               
                    g = g.ConfigureEnvironment(env => env
                        .UseNewtonsoftJson());
                
                """);
        }

        private sealed class EmptyTransformer : ITransformer
        {
            private readonly struct Option<T>
            {
                public static readonly Option<T> None = new();

                private Option(T value)
                {
                    Value = value;
                    HasValue = true;
                }

                public T Value { get; }
                public bool HasValue { get; }

                public static Option<T> From(T value) => new(value);
            }

            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, ConcurrentDictionary<(Type, Type, Type), Delegate>> ConversionDelegates = new();

            private readonly EmptyTransformer _recurse;
            private readonly ImmutableStack<IConverterFactory> _converterFactories;

            public EmptyTransformer(ImmutableStack<IConverterFactory> converterFactories)
            {
                _recurse = this;
                _converterFactories = converterFactories;
            }

            public EmptyTransformer(ImmutableStack<IConverterFactory> converterFactories, EmptyTransformer recurse) : this(converterFactories)
            {
                _recurse = recurse;
            }

            public ITransformer Add(IConverterFactory converterFactory)
            {
                return new EmptyTransformer(_converterFactories.Push(converterFactory));
            }

            public bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value)
            {
                if (source is { } actualSerialized)
                {
                    var maybeDeserializerDelegate = ConversionDelegates
                        .GetOrCreateValue(environment)
                        .GetOrAdd(
                            (typeof(TSource), actualSerialized.GetType(), typeof(TTarget)),
                            static (typeTuple, state) =>
                            {
                                var (@this, environment) = state;
                                var (staticSerializedType, actualSerializedType, requestedType) = typeTuple;

                                return (Delegate)typeof(EmptyTransformer)
                                    .GetMethod(nameof(GetTransformationFunction), BindingFlags.Instance | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(staticSerializedType, actualSerializedType, requestedType)
                                    .Invoke(@this, new object [] { environment })!;
                            },
                            (this, environment)) as Func<TSource, Option<TTarget>>;

                    if (maybeDeserializerDelegate is { } deserializerDelegate && deserializerDelegate(source) is { HasValue: true, Value: { } optionValue })
                    {
                        value = optionValue;
                        return true;
                    }

                    if (source is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }

            private Func<TStaticSource, Option<TTarget>> GetTransformationFunction<TStaticSource, TActualSource, TTarget>(IGremlinQueryEnvironment environment)
                where TActualSource : TStaticSource
            {
                IEnumerable<(IConverter<TActualSource, TTarget> converter, EmptyTransformer overridden)> Converters()
                {
                    var stack = _converterFactories;

                    while (!stack.IsEmpty)
                    {
                        var previousStack = stack.Pop(out var converterFactory);

                        if (converterFactory.TryCreate<TActualSource, TTarget>(environment) is { } converter)
                            yield return (converter, new EmptyTransformer(previousStack, this));

                        stack = previousStack;
                    }
                }

                var converters = Converters()
                    .ToArray();

                return (staticSerialized) =>
                {
                    if (staticSerialized is TActualSource actualSerialized)
                    {
                        foreach (var converter in converters)
                        {
                            if (converter.converter.TryConvert(actualSerialized, converter.overridden, _recurse, out var value))
                                return Option<TTarget>.From(value);
                        }
                    }

                    return Option<TTarget>.None;
                };
            }
        }

        public static readonly ITransformer Empty = new EmptyTransformer(ImmutableStack<IConverterFactory>.Empty);

        internal static readonly ITransformer Invalid = new InvalidTransformer();
    }
}
