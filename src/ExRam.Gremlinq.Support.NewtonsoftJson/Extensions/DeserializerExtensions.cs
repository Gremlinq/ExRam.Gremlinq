using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Xml;
using System.Numerics;
using ExRam.Gremlinq.Core.GraphElements;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class DeserializerExtensions
    {
        private static readonly Dictionary<string, Type> GraphSONTypes = new()
        {
            { "g:Int32", typeof(int) },
            { "g:Int64", typeof(long) },
            { "g:Float", typeof(float) },
            { "g:Double", typeof(double) },
            { "g:Direction", typeof(Direction) },
            { "g:Merge", typeof(Merge) },
            { "g:UUID", typeof(Guid) },
            { "g:Date", typeof(DateTimeOffset) },
            { "g:Timestamp", typeof(DateTimeOffset) },
            { "g:T", typeof(T) },

            //Extended
            { "gx:BigDecimal", typeof(decimal) },
            { "gx:Duration", typeof(TimeSpan) },
            { "gx:BigInteger", typeof(BigInteger) },
            { "gx:Byte",typeof(byte) },
            { "gx:ByteBuffer", typeof(byte[]) },
            { "gx:Char", typeof(char) },
            { "gx:Int16", typeof(short) }
        };

        private sealed class NewtonsoftJsonSerializerConverterFactory : IConverterFactory
        {
            private sealed class NewtonsoftJsonSerializerConverter<TSource, TTarget> : IConverter<TSource, TTarget>
                where TSource : JToken
            {
                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (source is TTarget alreadyRequestedValue)
                    {
                        value = alreadyRequestedValue;
                        return true;
                    }

                    if (source.ToObject<TTarget>(environment.GetJsonSerializer(recurse)) is { } requestedValue)
                    {
                        value = requestedValue;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(JToken).IsAssignableFrom(typeof(TSource))
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NewtonsoftJsonSerializerConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget)))
                    : null;
            }
        }

        private sealed class VertexOrEdgeConverterFactory : IConverterFactory
        {
            private sealed class VertexOrEdgeConverter<TTarget> : IConverter<JObject, TTarget>
            {
                public bool TryConvert(JObject jObject, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (jObject.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out var idToken) && jObject.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken) && labelToken.Type == JTokenType.String && jObject.TryGetValue("properties", out var propertiesToken) && propertiesToken is JObject propertiesObject)
                    {
                        if (recurse.TryTransform(propertiesObject, environment, out value))
                        {
                            value.SetIdAndLabel(idToken, labelToken, environment, recurse);
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JObject) && !typeof(TTarget).IsAssignableFrom(typeof(TSource)) && !typeof(Property).IsAssignableFrom(typeof(TTarget))
                    ? (IConverter<TSource, TTarget>)(object)new VertexOrEdgeConverter<TTarget>()
                    : default;
            }
        }

        private sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
        {
            private sealed class SingleItemArrayFallbackConverter<TSource, TTargetArray, TTargetArrayItem> : IConverter<TSource, TTargetArray>
            {
                public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTargetArray)) && recurse.TryTransform<TSource, TTargetArrayItem>(source, environment, out var typedValue))
                    {
                        value = (TTargetArray)(object)new[] { typedValue };
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TTarget).IsArray
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSource), typeof(TTarget), typeof(TTarget).GetElementType()!))
                    : default;
            }
        }

        private sealed class NullableConverterFactory : IConverterFactory
        {
            private sealed class NullableConverter<TTarget> : IConverter<JToken, TTarget?>
                where TTarget : struct
            {
                public bool TryConvert(JToken serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (serialized.Type == JTokenType.Null)
                    {
                        value = default(TTarget);
                        return true;
                    }

                    if (recurse.TryTransform<JToken, TTarget>(serialized, environment, out var requestedValue))
                    {
                        value = requestedValue;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JToken) && typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NullableConverter<>).MakeGenericType(typeof(TTarget).GetGenericArguments()[0]))
                    : default;
            }
        }

        private sealed class PropertyConverterFactory : IConverterFactory
        {
            private sealed class PropertyConverter<TTargetProperty, TTargetPropertyValue> : IConverter<JValue, TTargetProperty>
                where TTargetProperty : Property
            {
                public bool TryConvert(JValue serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetProperty? value)
                {
                    if (recurse.TryTransform<JValue, TTargetPropertyValue>(serialized, environment, out var propertyValue))
                    {
                        //TODO: Improvement opportunity.

                        if (Activator.CreateInstance(typeof(TTargetProperty), propertyValue) is TTargetProperty requestedProperty)
                        {
                            value = requestedProperty;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JValue) && typeof(Property).IsAssignableFrom(typeof(TTarget)) && typeof(TTarget).IsGenericType
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(PropertyConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetGenericArguments()[0]))
                    : default;
            }
        }

        private sealed class NativeTypeConverterFactory : IConverterFactory
        {
            public sealed class NativeTypeConverter<TTarget> : IConverter<JValue, TTarget>
            {
                public bool TryConvert(JValue serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
					if (serialized.Value is TTarget serializedValue)
                    {
                        value = serializedValue;
                        return true;
                    }

                    if (environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTarget)))
                    {
                        if (serialized.ToObject<TTarget>() is { } convertedSerializedValue)
                        {
                            value = convertedSerializedValue;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(JValue).IsAssignableFrom(typeof(TSource))
                    ? (IConverter<TSource, TTarget>)(object)new NativeTypeConverter<TTarget>()
                    : default;
            }
        }

        private sealed class ExpandoObjectConverterFactory : IConverterFactory
        {
            private sealed class ExpandoObjectConverter<TTarget> : IConverter<JObject, TTarget>
            {
                public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (recurse.TryTransform<JObject, JObject>(serialized, environment, out var strippedJObject))
                    {
                        var expando = new ExpandoObject();

                        foreach (var property in strippedJObject)
                        {
                            if (property.Value is { } propertyValue && recurse.TryTransform<JToken, object>(propertyValue, environment, out var item))
                                expando.TryAdd(property.Key, item);
                        }

                        value = (TTarget)(object)expando;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JObject) && typeof(TTarget).IsAssignableFrom(typeof(ExpandoObject))
                    ? (IConverter<TSource, TTarget>)(object)new ExpandoObjectConverter<TTarget>()
                    : default;
            }
        }

        private sealed class LabelLookupConverterFactory : IConverterFactory
        {
            private sealed class LabelLookupConverter<TTarget> : IConverter<JObject, TTarget>
            {
                public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    // Elements
                    var modelTypes = environment.GetCache().ModelTypesForLabels;
                    var label = serialized["label"]?.ToString();

                    var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(typeof(TTarget).IsAssignableFrom)
                        : default;

                    if (modelType != null && modelType != typeof(TTarget))
                    {
                        if (recurse.TryDeserialize(modelType).From(serialized, environment) is TTarget target)
                        {
                            value = target;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JObject) && !typeof(TSource).IsSealed
                    ? (IConverter<TSource, TTarget>)(object)new LabelLookupConverter<TTarget>()
                    : default;
            }
        }

        private sealed class VertexPropertyExtractConverterFactory : IConverterFactory
        {
            private sealed class VertexPropertyExtractConverter<TTarget> : IConverter<JObject, TTarget>
            {
                public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    var nativeTypes = environment.GetCache().FastNativeTypes;

                    if (nativeTypes.ContainsKey(typeof(TTarget)) || typeof(TTarget).IsEnum && nativeTypes.ContainsKey(typeof(TTarget).GetEnumUnderlyingType()))
                    {
                        if (serialized.TryGetValue("value", out var valueToken))
                        {
                            if (recurse.TryTransform(valueToken, environment, out value))
                                return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JObject)
                    ? (IConverter<TSource, TTarget>)(object)new VertexPropertyExtractConverter<TTarget>()
                    : default;
            }
        }

        private sealed class TypedValueConverterFactory : IConverterFactory
        {
            public sealed class TypedValueConverter<TTarget> : IConverter<JObject, TTarget>
            {
                public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (serialized.TryGetValue("@type", out var typeName) && serialized.TryGetValue("@value", out var valueToken))
                    {
                        if (typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && GraphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                        {
                            if (typeof(TTarget) != moreSpecificType && typeof(TTarget).IsAssignableFrom(moreSpecificType))
                            {
                                if (recurse.TryDeserialize(moreSpecificType).From(valueToken, environment) is TTarget target)
                                {
                                    value = target;
                                    return true;
                                }
                            }
                        }

                        return recurse.TryTransform(valueToken, environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JObject)
                    ? (IConverter<TSource, TTarget>)(object)new TypedValueConverter<TTarget>()
                    : default;
            }
        }

        private sealed class ConvertMapsConverterFactory : IConverterFactory
        {
            private sealed class ConvertMapsConverter<TTarget> : IConverter<JObject, TTarget>
            {
                public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (serialized.TryGetValue("@type", out var nestedType) && "g:Map".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray mapArray)
                        {
                            var retObject = new JObject();

                            for (var i = 0; i < mapArray.Count / 2; i++)
                            {
                                if (mapArray[i * 2] is JValue { Type: JTokenType.String } key)
                                    retObject.Add(key.Value<string>()!, mapArray[i * 2 + 1]);
                            }

                            return recurse.TryTransform(retObject, environment, out value);
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JObject)
                    ? (IConverter<TSource, TTarget>)(object)new ConvertMapsConverter<TTarget>()
                    : default;
            }
        }

        private sealed class BulkSetConverterFactory : IConverterFactory
        {
            private sealed class BulkSetConverter<TTargetArray, TTargetArrayItem> : IConverter<JObject, TTargetArray>
            {
                public bool TryConvert(JObject serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTargetArray)))
                    {
                        if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                            {
                                var array = new List<TTargetArrayItem>();

                                for (var i = 0; i < setArray.Count; i += 2)
                                {
                                    if (recurse.TryTransform<JToken, TTargetArrayItem>(setArray[i], environment, out var element))
                                    {
                                        if (recurse.TryTransform<JToken, int>(setArray[i + 1], environment, out var bulk) && bulk != 1)
                                        {
                                            for (var j = 0; j < bulk; j++)
                                            {
                                                array.Add(element);
                                            }
                                        }
                                        else
                                            array.Add(element);
                                    }
                                }

                                value = (TTargetArray)(object)array.ToArray();
                                return true;
                            }
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TTarget).IsArray && typeof(TSource) == typeof(JObject)
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(BulkSetConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!))
                    : default;
            }
        }

        private sealed class ArrayExtractConverterFactory : IConverterFactory
        {
            private sealed class ArrayExtractConverter<TTarget> : IConverter<JArray, TTarget>
            {
                public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if ((!typeof(TTarget).IsArray || environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTarget))) && !typeof(TTarget).IsInstanceOfType(serialized))
                    {
                        if (serialized.Count != 1)
                        {
                            value = serialized.Count == 0 && typeof(TTarget).IsClass
                                ? default!  //TODO: Drop NotNullWhen(true) ?
                                : throw new JsonReaderException($"Cannot convert array\r\n\r\n{serialized}\r\n\r\nto scalar value of type {typeof(TTarget)}.");

                            return true;
                        }

                        return recurse.TryTransform(serialized[0], environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JArray) 
                    ? (IConverter<TSource, TTarget>)(object)new ArrayExtractConverter<TTarget>()
                    : default;
            }
        }

        private sealed class ArrayLiftingConverterFactory : IConverterFactory
        {
            private sealed class ArrayLiftingConverter<TTarget> : IConverter<JArray, TTarget>
            {
                public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
                {
                    if (recurse.TryTransform<JArray, object[]>(serialized, environment, out var requested))
                    {
                        value = (TTarget)(object)requested;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JArray) && typeof(TTarget).IsAssignableFrom(typeof(object[]))
                    ? (IConverter<TSource, TTarget>)(object)new ArrayLiftingConverter<TTarget>()
                    : default;
            }
        }

        private sealed class TraverserConverterFactory : IConverterFactory
        {
            private sealed class TraverserConverter<TTargetArray, TTargetItem> : IConverter<JArray, TTargetArray>
            {
                public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTargetArray)))
                    {
                        var array = default(List<TTargetItem>);

                        for (var i = 0; i < serialized.Count; i++)
                        {
                            if (serialized[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TTargetItem>(environment, recurse) is { } enumerable)
                            {
                                array ??= new List<TTargetItem>(serialized.Count);

                                foreach (var item1 in enumerable)
                                {
                                    array.Add(item1);
                                }
                            }
                            else if (recurse.TryTransform<JToken, TTargetItem>(serialized[i], environment, out var item2))
                            {
                                array ??= new List<TTargetItem>(serialized.Count);

                                array.Add(item2);
                            }
                        }

                        value = (TTargetArray)(object)(array?.ToArray() ?? Array.Empty<TTargetItem>());
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
            {
                return typeof(TSource) == typeof(JArray) && typeof(TTarget).IsArray
                    ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(TraverserConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!))
                    : default;
            }
        }

        private sealed class TimeSpanConverterFactory : FixedTypeConverterFactory<TimeSpan>
        {
            protected override TimeSpan? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
            {
                return jValue.Type == JTokenType.String
                    ? XmlConvert.ToTimeSpan(jValue.Value<string>()!)
                    : default(TimeSpan?);
            }
        }

        private sealed class DateTimeOffsetConverterFactory : FixedTypeConverterFactory<DateTimeOffset>
        {
            protected override DateTimeOffset? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
            {
                return jValue.Value switch
                {
                    DateTime dateTime => new DateTimeOffset(dateTime),
                    DateTimeOffset dateTimeOffset => dateTimeOffset,
                    _ when jValue.Type == JTokenType.Integer => DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()),
                    _ => default(DateTimeOffset?)
                };
            }
        }

        private sealed class DateTimeConverterFactory : FixedTypeConverterFactory<DateTime>
        {
            protected override DateTime? Convert(JValue jValue, IGremlinQueryEnvironment environment, ITransformer recurse)
            {
                return jValue.Value switch
                {
                    DateTime dateTime => dateTime,
                    DateTimeOffset dateTimeOffset => dateTimeOffset.UtcDateTime,
                    string dateTimeString when DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parseResult) => parseResult,
                    _ when jValue.Type == JTokenType.Integer => new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()).Ticks, DateTimeKind.Utc),
                    _ => default(DateTime?)
                };
            }
        }

        private static readonly JsonSerializer Serializer = JsonSerializer.Create(
            new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });

        // ReSharper disable ConvertToLambdaExpression
        public static ITransformer AddNewtonsoftJson(this ITransformer deserializer) => deserializer
            .Add(ConverterFactory
                .Create<byte[], ResponseMessage<List<object>>>(static (message, env, recurse) =>
                {
                    var maybeResponseMessage = Serializer
                        .Deserialize<ResponseMessage<JToken>>(new JsonTextReader(new StreamReader(new MemoryStream(message))));

                    if (maybeResponseMessage is { } responseMessage)
                    {
                        return new ResponseMessage<List<object>>
                        {
                            RequestId = responseMessage.RequestId,
                            Status = responseMessage.Status,
                            Result = new ResponseResult<List<object>>
                            {
                                Data = new List<object>
                                {
                                    responseMessage.Result.Data
                                },
                                Meta = responseMessage.Result.Meta
                            }
                        };
                    }

                    return default;
                }))
            .Add(new NewtonsoftJsonSerializerConverterFactory())
            .Add(new VertexOrEdgeConverterFactory())
            .Add(new SingleItemArrayFallbackConverterFactory())
            .Add(new PropertyConverterFactory())
            .Add(new ExpandoObjectConverterFactory())  //TODO: Move
            .Add(new LabelLookupConverterFactory())
            .Add(new VertexPropertyExtractConverterFactory())
            .Add(new ArrayExtractConverterFactory())
            .Add(new ArrayLiftingConverterFactory())
            .Add(new TypedValueConverterFactory())
            .Add(new ConvertMapsConverterFactory())
            .Add(new BulkSetConverterFactory())
            .Add(new TraverserConverterFactory())
            .Add(new NullableConverterFactory())
            .Add(new NativeTypeConverterFactory())
            .Add(new TimeSpanConverterFactory())
            .Add(new DateTimeOffsetConverterFactory())
            .Add(new DateTimeConverterFactory());
        // ReSharper restore ConvertToLambdaExpression
    }
}
