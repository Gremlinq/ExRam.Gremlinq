using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using Gremlin.Net.Driver.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class TransformerExtensions
    {
        public readonly struct FluentForType
        {
            private readonly Type _type;
            private readonly ITransformer _deserializer;

            private static readonly ConcurrentDictionary<(Type, Type), Delegate?> FromClassDelegates = new();
            private static readonly ConcurrentDictionary<(Type, Type), Delegate?> FromStructDelegates = new();

            public FluentForType(ITransformer deserializer, Type type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = type.GetGenericArguments()[0];

                _type = type;
                _deserializer = deserializer;
            }

            public object? From<TSource>(TSource source, IGremlinQueryEnvironment environment)
            {
                return TryGetDelegate(typeof(TSource), _type) is Func<ITransformer, TSource, IGremlinQueryEnvironment, object?> fromDelegate
                    ? fromDelegate(_deserializer, source, environment)
                    : default;
            }

            private static Delegate? TryGetDelegate(Type sourceType, Type targetType)
            {
                var delegatesDict = targetType.IsValueType
                    ? FromStructDelegates
                    : FromClassDelegates;

                return delegatesDict
                    .GetOrAdd(
                        (sourceType, targetType),
                        static tuple =>
                        {
                            var (sourceType, targetType) = tuple;

                            var methodName = targetType.IsValueType
                                ? nameof(FromStruct)
                                : nameof(FromClass);

                            return typeof(FluentForType)
                                .GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)!
                                .MakeGenericMethod(sourceType, targetType)
                                .Invoke(null, Array.Empty<object>()) as Delegate;
                        });
            }

            private static Func<ITransformer, TSource, IGremlinQueryEnvironment, object?> FromClass<TSource, TTarget>()
                where TTarget : class => (deserializer, serialized, environment) => deserializer.TryTransform<TSource, TTarget>(serialized, environment, out var value)
                    ? value
                    : default;

            private static Func<ITransformer, TSource, IGremlinQueryEnvironment, object?> FromStruct<TSource, TTarget>()
                where TTarget : struct => (deserializer, serialized, environment) => deserializer.TryTransform<TSource, TTarget>(serialized, environment, out var value)
                    ? value
                    : default(TTarget?);
        }

        private static readonly JsonSerializer jsonSerializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });

        private static readonly ConditionalWeakTable<ITransformer, ITransformer> ShortcutTransformers = new();

        public static ITransformer UseNewtonsoftJson(this ITransformer transformer)
        {
            return transformer
                .Add(ConverterFactory
                    .Create<string, JToken>((str, env, recurse) => jsonSerializer
                        .Deserialize<JToken>(new JsonTextReader(new StringReader(str)))))
                .Add(ConverterFactory
                    .Create<byte[], JToken>((bytes, env, recurse) => jsonSerializer
                        .Deserialize<JToken>(new JsonTextReader(new StreamReader(new MemoryStream(bytes))))))
                .Add(ConverterFactory
                    .Create<byte[], ResponseMessage<List<object>>>((message, env, recurse) =>
                    {
                        var token = recurse
                            .TransformTo<JToken>()
                            .From(message, env);

                        return ShortcutTransformers
                            .GetValue(
                                recurse,
                                static transformer => transformer
                                    .Add(ConverterFactory
                                        .Create<JToken, JToken>((token, env, recurse) => token)))
                            .TransformTo<ResponseMessage<List<object>>>()
                            .From(token, env);
                    }))
                .Add(new NewtonsoftJsonSerializerConverterFactory())
                .Add(new VertexOrEdgeConverterFactory())
                .Add(new SingleItemArrayFallbackConverterFactory())
                .Add(new PropertyConverterFactory())
                .Add(new ExpandoObjectConverterFactory())  //TODO: Move
                .Add(new LabelLookupConverterFactory())
                .Add(new VertexPropertyExtractConverterFactory())
                .Add(new TypedValueConverterFactory())
                .Add(new MapConverterFactory())
                .Add(new BulkSetConverterFactory())
                .Add(new EnumerableConverterFactory())
                .Add(new NullableConverterFactory())
                .Add(new NativeTypeConverterFactory())
                .Add(new TimeSpanConverterFactory())
                .Add(new DateTimeOffsetConverterFactory())
                .Add(new DateTimeConverterFactory());
        }

        public static FluentForType TryTransformTo(this ITransformer deserializer, Type type) => new(deserializer, type);
    }
}
