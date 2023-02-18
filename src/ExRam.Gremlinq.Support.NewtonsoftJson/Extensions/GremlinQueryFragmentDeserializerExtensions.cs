using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Xml;
using System.Numerics;
using ExRam.Gremlinq.Core.GraphElements;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializerExtensions
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

        private sealed class NewtonsoftJsonSerializerDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class NewtonsoftJsonSerializerDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
                where TSerialized : JToken
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized is TRequested alreadyRequestedValue)
                    {
                        value = alreadyRequestedValue;
                        return true;
                    }

                    if (serialized.ToObject<TRequested>(environment.GetJsonSerializer(recurse)) is { } requestedValue)
                    {
                        value = requestedValue;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JToken).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(NewtonsoftJsonSerializerDeserializationTransformation<,>).MakeGenericType(typeof(TSerialized), typeof(TRequested)))
                    : null;
            }
        }

        private sealed class VertexOrEdgeDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class VertexOrEdgeDeserializationTransformation<TRequested> : IDeserializationTransformation<JObject, TRequested>
            {
                public bool Transform(JObject jObject, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (jObject.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out var idToken) && jObject.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken) && labelToken.Type == JTokenType.String && jObject.TryGetValue("properties", out var propertiesToken) && propertiesToken is JObject propertiesObject)
                    {
                        if (recurse.TryDeserialize(propertiesObject, environment, out value))
                        {
                            value.SetIdAndLabel(idToken, labelToken, environment, recurse);
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JObject) && !typeof(TRequested).IsAssignableFrom(typeof(TSerialized)) && !typeof(Property).IsAssignableFrom(typeof(TRequested))
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new VertexOrEdgeDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class SingleItemArrayFallbackDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class SingleItemArrayFallbackDeserializationTransformation<TSerialized, TRequestedArray, TRequestedArrayItem> : IDeserializationTransformation<TSerialized, TRequestedArray>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequestedArray)) && recurse.TryDeserialize<TSerialized, TRequestedArrayItem>(serialized, environment, out var typedValue))
                    {
                        value = (TRequestedArray)(object)new[] { typedValue };
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class NullableDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class NullableDeserializationTransformation<TRequested> : IDeserializationTransformation<JToken, TRequested?>
                where TRequested : struct
            {
                public bool Transform(JToken serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized.Type == JTokenType.Null)
                    {
                        value = default(TRequested);
                        return true;
                    }

                    if (recurse.TryDeserialize<JToken, TRequested>(serialized, environment, out var requestedValue))
                    {
                        value = requestedValue;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JToken) && typeof(TRequested).IsGenericType && typeof(TRequested).GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(NullableDeserializationTransformation<>).MakeGenericType(typeof(TRequested).GetGenericArguments()[0]))
                    : default;
            }
        }

        private sealed class PropertyDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class PropertyDeserializationTransformation<TRequestedProperty, TRequestedPropertyValue> : IDeserializationTransformation<JValue, TRequestedProperty>
                where TRequestedProperty : Property
            {
                public bool Transform(JValue serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedProperty? value)
                {
                    if (recurse.TryDeserialize<JValue, TRequestedPropertyValue>(serialized, environment, out var propertyValue))
                    {
                        //TODO: Improvement opportunity.

                        if (Activator.CreateInstance(typeof(TRequestedProperty), propertyValue) is TRequestedProperty requestedProperty)
                        {
                            value = requestedProperty;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JValue) && typeof(Property).IsAssignableFrom(typeof(TRequested)) && typeof(TRequested).IsGenericType
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(PropertyDeserializationTransformation<,>).MakeGenericType(typeof(TRequested), typeof(TRequested).GetGenericArguments()[0]))
                    : default;
            }
        }

        private sealed class NativeTypeDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            public sealed class NativeTypeDeserializationTransformation<TRequested> : IDeserializationTransformation<JValue, TRequested>
            {
                public bool Transform(JValue serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
					if (serialized.Value is TRequested serializedValue)
                    {
                        value = serializedValue;
                        return true;
                    }

                    if (environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequested)))
                    {
                        if (serialized.ToObject<TRequested>() is { } convertedSerializedValue)
                        {
                            value = convertedSerializedValue;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(JValue).IsAssignableFrom(typeof(TSerialized))
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new NativeTypeDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class ExpandoObjectDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ExpandoObjectDeserializationTransformation<TRequested> : IDeserializationTransformation<JObject, TRequested>
            {
                public bool Transform(JObject serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (recurse.TryDeserialize<JObject, JObject>(serialized, environment, out var processedFragment))
                    {
                        var expando = new ExpandoObject();

                        foreach (var property in processedFragment)
                        {
                            if (property.Value is { } propertyValue && recurse.TryDeserialize<JToken, object>(propertyValue, environment, out var item))
                                expando.TryAdd(property.Key, item);
                        }

                        value = (TRequested)(object)expando;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JObject) && typeof(TRequested).IsAssignableFrom(typeof(ExpandoObject))
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new ExpandoObjectDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class LabelLookupDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class LabelLookupDeserializationTransformation<TRequested> : IDeserializationTransformation<JObject, TRequested>
            {
                public bool Transform(JObject serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    // Elements
                    var modelTypes = environment.GetCache().ModelTypesForLabels;
                    var label = serialized["label"]?.ToString();

                    var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                        ? types.FirstOrDefault(typeof(TRequested).IsAssignableFrom)
                        : default;

                    if (modelType != null && modelType != typeof(TRequested))
                    {
                        if (recurse.TryDeserialize(modelType).From(serialized, environment) is TRequested requested)
                        {
                            value = requested;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JObject) && !typeof(TSerialized).IsSealed
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new LabelLookupDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class VertexPropertyExtractDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class VertexPropertyExtractDeserializationTransformation<TRequested> : IDeserializationTransformation<JObject, TRequested>
            {
                public bool Transform(JObject serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    var nativeTypes = environment.GetCache().FastNativeTypes;

                    if (nativeTypes.ContainsKey(typeof(TRequested)) || typeof(TRequested).IsEnum && nativeTypes.ContainsKey(typeof(TRequested).GetEnumUnderlyingType()))
                    {
                        if (serialized.TryGetValue("value", out var valueToken))
                        {
                            if (recurse.TryDeserialize(valueToken, environment, out value))
                                return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JObject)
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new VertexPropertyExtractDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class TypedValueDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            public sealed class TypedValueDeserializationTransformation<TRequested> : IDeserializationTransformation<JObject, TRequested>
            {
                public bool Transform(JObject serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized.TryGetValue("@type", out var typeName) && serialized.TryGetValue("@value", out var valueToken))
                    {
                        if (typeName.Type == JTokenType.String && typeName.Value<string>() is { } typeNameString && GraphSONTypes.TryGetValue(typeNameString, out var moreSpecificType))
                        {
                            if (typeof(TRequested) != moreSpecificType && typeof(TRequested).IsAssignableFrom(moreSpecificType))
                            {
                                if (recurse.TryDeserialize(moreSpecificType).From(valueToken, environment) is TRequested requested)
                                {
                                    value = requested;
                                    return true;
                                }
                            }
                        }

                        return recurse.TryDeserialize(valueToken, environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JObject)
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new TypedValueDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class ConvertMapsDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ConvertMapsDeserializationTransformation<TRequested> : IDeserializationTransformation<JObject, TRequested>
            {
                public bool Transform(JObject serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
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

                            return recurse.TryDeserialize(retObject, environment, out value);
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JObject)
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new ConvertMapsDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class BulkSetDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class BulkSetDeserializationTransformation<TRequestedArray, TRequestedItem> : IDeserializationTransformation<JObject, TRequestedArray>
            {
                public bool Transform(JObject serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequestedArray)))
                    {
                        if (serialized.TryGetValue("@type", out var typeToken) && "g:BulkSet".Equals(typeToken.Value<string>(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (serialized.TryGetValue("@value", out var valueToken) && valueToken is JArray setArray)
                            {
                                var array = new List<TRequestedItem>();

                                for (var i = 0; i < setArray.Count; i += 2)
                                {
                                    if (recurse.TryDeserialize<JToken, TRequestedItem>(setArray[i], environment, out var element))
                                    {
                                        if (recurse.TryDeserialize<JToken, int>(setArray[i + 1], environment, out var bulk) && bulk != 1)
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

                                value = (TRequestedArray)(object)array.ToArray();
                                return true;
                            }
                        }
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray && typeof(TSerialized) == typeof(JObject)
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(BulkSetDeserializationTransformation<,>).MakeGenericType(typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class ArrayExtractDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ArrayExtractDeserializationTransformation<TRequested> : IDeserializationTransformation<JArray, TRequested>
            {
                public bool Transform(JArray serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if ((!typeof(TRequested).IsArray || environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequested))) && !typeof(TRequested).IsInstanceOfType(serialized))
                    {
                        if (serialized.Count != 1)
                        {
                            value = serialized.Count == 0 && typeof(TRequested).IsClass
                                ? default!  //TODO: Drop NotNullWhen(true) ?
                                : throw new JsonReaderException($"Cannot convert array\r\n\r\n{serialized}\r\n\r\nto scalar value of type {typeof(TRequested)}.");

                            return true;
                        }

                        return recurse.TryDeserialize(serialized[0], environment, out value);
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JArray) 
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new ArrayExtractDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class ArrayLiftingDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class ArrayLiftingDeserializationTransformation<TRequested> : IDeserializationTransformation<JArray, TRequested>
            {
                public bool Transform(JArray serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (recurse.TryDeserialize<JArray, object[]>(serialized, environment, out var requested))
                    {
                        value = (TRequested)(object)requested;
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JArray) && typeof(TRequested).IsAssignableFrom(typeof(object[]))
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new ArrayLiftingDeserializationTransformation<TRequested>()
                    : default;
            }
        }

        private sealed class TraverserDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class TraverserDeserializationTransformation<TRequestedArray, TRequestedItem> : IDeserializationTransformation<JArray, TRequestedArray>
            {
                public bool Transform(JArray serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TRequestedArray)))
                    {
                        var array = default(List<TRequestedItem>);

                        for (var i = 0; i < serialized.Count; i++)
                        {
                            if (serialized[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TRequestedItem>(environment, recurse) is { } enumerable)
                            {
                                array ??= new List<TRequestedItem>(serialized.Count);

                                foreach (var item1 in enumerable)
                                {
                                    array.Add(item1);
                                }
                            }
                            else if (recurse.TryDeserialize<JToken, TRequestedItem>(serialized[i], environment, out var item2))
                            {
                                array ??= new List<TRequestedItem>(serialized.Count);

                                array.Add(item2);
                            }
                        }

                        value = (TRequestedArray)(object)(array?.ToArray() ?? Array.Empty<TRequestedItem>());
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TSerialized) == typeof(JArray) && typeof(TRequested).IsArray
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(TraverserDeserializationTransformation<,>).MakeGenericType(typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        private sealed class TimeSpanDeserializationTransformationFactory : FixedTypeDeserializationTransformationFactory<TimeSpan>
        {
            protected override TimeSpan? Convert(JValue jValue, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
            {
                return jValue.Type == JTokenType.String
                    ? XmlConvert.ToTimeSpan(jValue.Value<string>()!)
                    : default(TimeSpan?);
            }
        }

        private sealed class DateTimeOffsetDeserializationTransformationFactory : FixedTypeDeserializationTransformationFactory<DateTimeOffset>
        {
            protected override DateTimeOffset? Convert(JValue jValue, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
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

        private sealed class DateTimeDeserializationTransformationFactory : FixedTypeDeserializationTransformationFactory<DateTime>
        {
            protected override DateTime? Convert(JValue jValue, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
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

        // ReSharper disable ConvertToLambdaExpression
        public static IGremlinQueryFragmentDeserializer AddNewtonsoftJson(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Add(new NewtonsoftJsonSerializerDeserializationTransformationFactory())
            .Add(new VertexOrEdgeDeserializationTransformationFactory())
            .Add(new SingleItemArrayFallbackDeserializationTransformationFactory())
            .Add(new PropertyDeserializationTransformationFactory())
            .Add(new ExpandoObjectDeserializationTransformationFactory())  //TODO: Move
            .Add(new LabelLookupDeserializationTransformationFactory())
            .Add(new VertexPropertyExtractDeserializationTransformationFactory())
            .Add(new ArrayExtractDeserializationTransformationFactory())
            .Add(new ArrayLiftingDeserializationTransformationFactory())
            .Add(new TypedValueDeserializationTransformationFactory())
            .Add(new ConvertMapsDeserializationTransformationFactory())
            .Add(new BulkSetDeserializationTransformationFactory())
            .Add(new TraverserDeserializationTransformationFactory())
            .Add(new NullableDeserializationTransformationFactory())
            .Add(new NativeTypeDeserializationTransformationFactory())
            .Add(new TimeSpanDeserializationTransformationFactory())
            .Add(new DateTimeOffsetDeserializationTransformationFactory())
            .Add(new DateTimeDeserializationTransformationFactory());
        // ReSharper restore ConvertToLambdaExpression
    }
}
