using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryExecutionResultDeserializer
    {
        private static readonly ConditionalWeakTable<IGraphModel, IDictionary<string, Type[]>> ModelTypes = new ConditionalWeakTable<IGraphModel, IDictionary<string, Type[]>>();
        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, ConditionalWeakTable<IQueryFragmentDeserializer, JsonSerializer>> PopulatingSerializers = new ConditionalWeakTable<IGremlinQueryEnvironment, ConditionalWeakTable<IQueryFragmentDeserializer, JsonSerializer>>();
        private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, ConditionalWeakTable<IQueryFragmentDeserializer, JsonSerializer>> IgnoringSerializers = new ConditionalWeakTable<IGremlinQueryEnvironment, ConditionalWeakTable<IQueryFragmentDeserializer, JsonSerializer>>();

        private sealed class VertexImpl : IVertex
        {
            public object? Id { get; set; }
        }

        private sealed class EdgeImpl : IEdge
        {
            public object? Id { get; set; }
        }

        private sealed class GraphsonJsonSerializer : JsonSerializer
        {
            #region Nested
            private sealed class GremlinContractResolver : DefaultContractResolver
            {
                private sealed class EmptyListValueProvider : IValueProvider
                {
                    private readonly object _defaultValue;
                    private readonly IValueProvider _innerProvider;

                    public EmptyListValueProvider(IValueProvider innerProvider, Type elementType)
                    {
                        _innerProvider = innerProvider;
                        _defaultValue = Array.CreateInstance(elementType, 0);
                    }

                    public void SetValue(object target, object? value)
                    {
                        _innerProvider.SetValue(target, value ?? _defaultValue);
                    }

                    public object GetValue(object target)
                    {
                        return _innerProvider.GetValue(target) ?? _defaultValue;
                    }
                }

                private sealed class EmptyDictionaryValueProvider : IValueProvider
                {
                    private readonly object _defaultValue;
                    private readonly IValueProvider _innerProvider;

                    public EmptyDictionaryValueProvider(IValueProvider innerProvider)
                    {
                        _innerProvider = innerProvider;
                        _defaultValue = new Dictionary<string, object>();
                    }

                    public void SetValue(object target, object? value)
                    {
                        _innerProvider.SetValue(target, value ?? _defaultValue);
                    }

                    public object GetValue(object target)
                    {
                        return _innerProvider.GetValue(target) ?? _defaultValue;
                    }
                }

                private readonly IGraphElementPropertyModel _model;

                public GremlinContractResolver(IGraphElementPropertyModel model)
                {
                    _model = model;
                }

                protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);

                    if (_model.Metadata.TryGetValue(member, out var name))
                        property.PropertyName = name.Name;

                    return property;
                }

                protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
                {
                    var provider = base.CreateMemberValueProvider(member);

                    if (member is PropertyInfo propertyMember)
                    {
                        var propertyType = propertyMember.PropertyType;

                        if (propertyType == typeof(IDictionary<string, object>) && propertyMember.Name == nameof(VertexProperty<object>.Properties) && typeof(IVertexProperty).IsAssignableFrom(propertyMember.DeclaringType))
                            return new EmptyDictionaryValueProvider(provider);

                        if (propertyType.IsArray)
                            return new EmptyListValueProvider(provider, propertyType.GetElementType());
                    }

                    return provider;
                }
            }

            private sealed class JTokenConverterConverter : JsonConverter
            {
                private readonly IGremlinQueryEnvironment _environment;
                private readonly IQueryFragmentDeserializer _deserializer;

                [ThreadStatic]
                // ReSharper disable once StaticMemberInGenericType
                private static bool _canConvert;

                public JTokenConverterConverter(
                    IQueryFragmentDeserializer deserializer,
                    IGremlinQueryEnvironment environment)
                {
                    _deserializer = deserializer;
                    _environment = environment;
                }

                public override bool CanConvert(Type objectType)
                {
                    if (!_canConvert)
                    {
                        _canConvert = true;

                        return false;
                    }

                    return true;
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    throw new NotSupportedException($"Cannot write to {nameof(JTokenConverterConverter)}.");
                }

                public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
                {
                    var token = JToken.Load(reader);

                    try
                    {
                        _canConvert = false;

                        return _deserializer.TryDeserialize(token, objectType, _environment);
                    }
                    finally
                    {
                        _canConvert = true;
                    }
                }
            }
            #endregion

            public GraphsonJsonSerializer(
                DefaultValueHandling defaultValueHandling,
                IGremlinQueryEnvironment environment,
                IQueryFragmentDeserializer fragmentDeserializer)
            {
                DefaultValueHandling = defaultValueHandling;
                ContractResolver = new GremlinContractResolver(environment.Model.PropertiesModel);
                Converters.Add(new JTokenConverterConverter(fragmentDeserializer, environment));
            }
        }

        private sealed class GremlinQueryExecutionResultDeserializerImpl : IGremlinQueryExecutionResultDeserializer
        {
            private readonly IQueryFragmentDeserializer _fragmentSerializer;

            public GremlinQueryExecutionResultDeserializerImpl(IQueryFragmentDeserializer fragmentSerializer)
            {
                _fragmentSerializer = fragmentSerializer;
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                var result = _fragmentSerializer
                    .TryDeserialize(executionResult, typeof(TElement[]), environment);

                return result switch
                {
                    TElement[] elements => elements.ToAsyncEnumerable(),
                    IAsyncEnumerable<TElement> enumerable => enumerable,
                    TElement element => AsyncEnumerableEx.Return(element),
                    IEnumerable enumerable => enumerable.Cast<TElement>().ToAsyncEnumerable(),
                    { } obj => throw new InvalidCastException($"A result of type {obj.GetType()} can't be interpreted as {nameof(IAsyncEnumerable<TElement>)}."),
                    _ => AsyncEnumerable.Empty<TElement>()
                };
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                return new GremlinQueryExecutionResultDeserializerImpl(transformation(_fragmentSerializer));
            }
        }

        private sealed class InvalidQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerableEx.Throw<TElement>(new InvalidOperationException($"{nameof(Deserialize)} must not be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQueryExecutionResultDeserializer)} on your {nameof(GremlinQuerySource)}."));
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}.");
            }
        }

        private sealed class ToStringGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                    throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                return AsyncEnumerableEx.Return((TElement)(object)result.ToString());
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(GremlinQueryExecutionResultDeserializer.ToString)}.");
            }
        }

        private sealed class ToGraphsonGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                    throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                return AsyncEnumerableEx.Return((TElement)(object)new GraphSON2Writer().WriteObject(result));
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IQueryFragmentDeserializer, IQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(ToGraphsonString)}.");
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer Identity = new GremlinQueryExecutionResultDeserializerImpl(QueryFragmentDeserializer.Identity);

        public static readonly IGremlinQueryExecutionResultDeserializer Invalid = new InvalidQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer ToGraphsonString = new ToGraphsonGremlinQueryExecutionResultDeserializer();

        public static new readonly IGremlinQueryExecutionResultDeserializer ToString = new ToStringGremlinQueryExecutionResultDeserializer();

        // ReSharper disable ConvertToLambdaExpression
        public static readonly IGremlinQueryExecutionResultDeserializer FromJToken = new GremlinQueryExecutionResultDeserializerImpl(QueryFragmentDeserializer
            .Identity
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                var populatingSerializer = PopulatingSerializers
                    .GetValue(
                        env,
                        closureEnv => new ConditionalWeakTable<IQueryFragmentDeserializer, JsonSerializer>())
                    .GetValue(
                        recurse,
                        closureRecurse => new GraphsonJsonSerializer(
                            DefaultValueHandling.Populate,
                            env,
                            recurse));

                var ignoringSerializer = IgnoringSerializers
                    .GetValue(
                        env,
                        closureEnv => new ConditionalWeakTable<IQueryFragmentDeserializer, JsonSerializer>())
                    .GetValue(
                        recurse,
                        closureRecurse => new GraphsonJsonSerializer(
                            DefaultValueHandling.Ignore,
                            env,
                            recurse));

                var ret = jToken.ToObject(
                    type,
                    populatingSerializer);

                if (!(ret is JToken) && jToken is JObject element)
                {
                    if (element.ContainsKey("id") && element.TryGetValue("label", out var label) && label.Type == JTokenType.String && element["properties"] is JObject propertiesToken)
                    {
                        if (propertiesToken.TryUnmap() is { } jObject)
                            propertiesToken = jObject;

                        ignoringSerializer.Populate(new JTokenReader(propertiesToken), ret);
                    }
                }

                if (ret is JObject newJObject)
                {
                    foreach (var property in newJObject)
                    {
                        if (recurse.TryDeserialize(newJObject[property.Key], typeof(JToken), env) is JToken newToken)
                            newJObject[property.Key] = newToken;
                    }
                }

                return ret;
            })
            .Override<JToken>((jToken, type, env, overridden, recurse) =>
            {
                if (!type.IsArray)
                {
                    if (!type.IsInstanceOfType(jToken))
                    {
                        var itemType = default(Type);

                        if (type.IsGenericType)
                        {
                            var genericTypeDefinition = type.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                                itemType = type.GetGenericArguments()[0];
                        }

                        switch (jToken)
                        {
                            case JArray array when array.Count != 1:
                            {
                                if (array.Count == 0 && (type.IsClass || itemType != null))
                                {
                                    return default;
                                }

                                throw new JsonReaderException($"Cannot convert array\r\n\r\n{array}\r\n\r\nto scalar value of type {type}.");
                            }
                            case JArray array:
                                return recurse.TryDeserialize(array[0], itemType ?? type, env);
                            case JValue jValue when jValue.Value == null:
                                return null;
                            case JValue jValue when itemType != null:
                                return recurse.TryDeserialize(jValue, itemType, env);
                        }
                    }
                }

                return overridden(jToken);
            })
            .Override<JValue>((jToken, type, env, overridden, recurse) =>
            {
                return typeof(Property).IsAssignableFrom(type) && type.IsGenericType
                    ? Activator.CreateInstance(type, recurse.TryDeserialize(jToken, type.GetGenericArguments()[0], env))
                    : overridden(jToken);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                return type == typeof(TimeSpan)
                    ? XmlConvert.ToTimeSpan(jValue.Value<string>())
                    : overridden(jValue);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                if (type == typeof(DateTimeOffset))
                {
                    switch (jValue.Value)
                    {
                        case DateTime dateTime:
                            return new DateTimeOffset(dateTime);
                        case DateTimeOffset dateTimeOffset:
                            return dateTimeOffset;
                        default:
                        {
                            if (jValue.Type == JTokenType.Integer)
                                return DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>());

                            break;
                        }
                    }
                }

                return overridden(jValue);
            })
            .Override<JValue>((jValue, type, env, overridden, recurse) =>
            {
                if (type == typeof(DateTime))
                {
                    switch (jValue.Value)
                    {
                        case DateTime dateTime:
                            return dateTime;
                        case DateTimeOffset dateTimeOffset:
                            return dateTimeOffset.UtcDateTime;
                    }

                    if (jValue.Type == JTokenType.Integer)
                        return new DateTime(DateTimeOffset.FromUnixTimeMilliseconds(jValue.Value<long>()).Ticks, DateTimeKind.Utc);
                }

                return overridden(jValue);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                // Elements
                var modelTypes = ModelTypes.GetValue(
                    env.Model,
                    closureModel =>
                    {
                        return closureModel
                            .VerticesModel
                            .Metadata
                            .Concat(closureModel.EdgesModel.Metadata)
                            .GroupBy(x => x.Value.Label)
                            .ToDictionary(
                                group => group.Key,
                                group => group
                                    .Select(x => x.Key)
                                    .ToArray(),
                                StringComparer.OrdinalIgnoreCase);
                    });


                var label = jObject["label"]?.ToString();

                var modelType = label != null && modelTypes.TryGetValue(label, out var types)
                    ? types.FirstOrDefault(type => type.IsAssignableFrom(type))
                    : default;

                if (modelType == null)
                {
                    if (type == typeof(IVertex))
                        modelType = typeof(VertexImpl);
                    else if (type == typeof(IEdge))
                        modelType = typeof(EdgeImpl);
                }

                if (modelType != null && modelType != type)
                    return recurse.TryDeserialize(jObject, modelType, env);

                return overridden(jObject);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                //Vertex Properties
                var nativeTypes = env.Model.NativeTypes;

                if (nativeTypes.Contains(type) || (type.IsEnum && nativeTypes.Contains(type.GetEnumUnderlyingType())))
                {
                    if (jObject.ContainsKey("value"))
                        return recurse.TryDeserialize(jObject["value"], type, env);
                }

                return overridden(jObject);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                //@type == "g:Map"
                if (jObject.ContainsKey("@type") && jObject.TryGetValue("@value", out var valueToken))
                    return recurse.TryDeserialize(valueToken, type, env);

                return overridden(jObject);
            })
            .Override<JObject>((jObject, type, env, overridden, recurse) =>
            {
                //@type == "g:Map"
                return jObject.TryUnmap() is { } unmappedObject
                    ? recurse.TryDeserialize(unmappedObject, type, env)
                    : overridden(jObject);
            })
            .Override<JArray>((jArray, type, env, overridden, recurse) =>
            {
                //Traversers
                if (!type.IsArray)
                    return overridden(jArray);

                var array = default(ArrayList);
                var elementType = type.GetElementType();

                for (var i = 0; i < jArray.Count; i++)
                {
                    var bulk = 1;
                    var effectiveArrayItem = jArray[i];

                    if (effectiveArrayItem is JObject traverserObject && traverserObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && traverserObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
                    {
                        if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken) && recurse.TryDeserialize(bulkToken, typeof(int), env) is int bulkObject)
                            bulk = bulkObject;

                        if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                            effectiveArrayItem = traverserValue;
                    }

                    if (recurse.TryDeserialize(effectiveArrayItem, elementType, env) is { } item)
                    {
                        if (jArray.Count == 1 && bulk == 1)
                        {
                            var ret = Array.CreateInstance(elementType, 1);
                            ret.SetValue(item, 0);

                            return ret;
                        }

                        array ??= new ArrayList(jArray.Count);

                        for (var j = 0; j < bulk; j++)
                        {
                            array.Add(item);
                        }
                    }
                }

                return array?.ToArray(elementType) ?? Array.CreateInstance(elementType, 0);
            }));
            // ReSharper restore ConvertToLambdaExpression
    }
}
