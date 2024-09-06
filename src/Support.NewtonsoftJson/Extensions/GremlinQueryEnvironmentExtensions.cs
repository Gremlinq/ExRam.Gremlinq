using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    public static class GremlinQueryEnvironmentExtensions
    {
        private sealed class NativeTypeSerializerConverterFactory<TNative, TSerialized> : IConverterFactory
        {
            private sealed class NativeTypeSerializerConverter<TTarget> : IConverter<TNative, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> _serializer;

                public NativeTypeSerializerConverter(Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> serializer, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _serializer = serializer;
                }

                public bool TryConvert(TNative source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (_serializer(source, _environment, defer, recurse) is TTarget serialized)
                    {
                        value = serialized;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            private readonly Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> _serializer;

            public NativeTypeSerializerConverterFactory(Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> serializer)
            {
                _serializer = serializer;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(TNative) && typeof(TTarget).IsAssignableFrom(typeof(TSerialized))
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NativeTypeSerializerConverter<>).MakeGenericType(typeof(TNative), typeof(TSerialized), typeof(TTarget)), _serializer, environment)
                : default;
        }

        private sealed class NativeTypeDeserializerConverterFactory<TNative> : IConverterFactory
        {
            private sealed class NativeTypeDeserializerConverter<TTarget> : IConverter<JValue, TTarget>
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> _deserializer;

                public NativeTypeDeserializerConverter(Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> deserializer, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _deserializer = deserializer;
                }

                public bool TryConvert(JValue source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (_deserializer(source, _environment, defer, recurse) is TTarget deserialized)
                    {
                        value = deserialized;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            private readonly Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> _deserializer;

            public NativeTypeDeserializerConverterFactory(Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> deserializer)
            {
                _deserializer = deserializer;
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => typeof(TSource) == typeof(JValue) && typeof(TTarget) == typeof(TNative)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NativeTypeDeserializerConverter<>).MakeGenericType(typeof(TNative), typeof(TTarget)), _deserializer, environment)
                : default;
        }

        public static IGremlinQueryEnvironment UseNewtonsoftJson(this IGremlinQueryEnvironment environment) => environment
            .ConfigureDeserializer(deserializer => deserializer
                .Add(new DeferToNewtonsoftConverterFactory())
                .Add(new NewtonsoftJsonSerializerConverterFactory())
                .Add(new VertexPropertyPropertiesConverterFactory())
                .Add(new DictionaryConverterFactory())
                .Add(new DynamicObjectConverterFactory())

                .Add(new ExtractPropertyValueConverterFactory())
                .Add(new ScalarToPropertyConverterFactory())
                .Add(new PropertyHeuristicConverterFactory())

                .Add(new VertexOrEdgeConverterFactory())
                .Add(new LabelLookupConverterFactory())

                .Add(new TypedValueConverterFactory())
                .Add(new MapDeferralConverterFactory())
                .Add(new MapToDictionaryConverterFactory())
                .Add(new BulkSetConverterFactory())
                .Add(new EnumerableConverterFactory())

                .Add(new NativeTypeConverterFactory())
                .Add(new NullableConverterFactory())

                .Add(new TimeSpanConverterFactory())
                .Add(new DateTimeOffsetConverterFactory())
                .Add(new DateTimeConverterFactory()));

        public static IGremlinQueryEnvironment RegisterNativeType<TNative, TSerialized>(this IGremlinQueryEnvironment environment, Func<TNative, IGremlinQueryEnvironment, ITransformer, ITransformer, TSerialized> serializer, Func<JValue, IGremlinQueryEnvironment, ITransformer, ITransformer, TNative> deserializer)
        {
            return environment
                .ConfigureNativeTypes(_ => _
                    .Add(typeof(TNative)))
                .ConfigureSerializer(_ => _
                    .Add(new NativeTypeSerializerConverterFactory<TNative, TSerialized>(serializer)))
                .ConfigureDeserializer(_ => _
                    .Add(new NativeTypeDeserializerConverterFactory<TNative>(deserializer)));
        }
    }
}
