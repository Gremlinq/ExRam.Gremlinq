using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using Newtonsoft.Json;
using ExRam.Gremlinq.Core.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using System.Runtime.CompilerServices;
using System.Collections;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class NewtonsoftJsonSerializerConverterFactory : IConverterFactory
    {
        private sealed class GraphsonJsonSerializer : JsonSerializer
        {
            private sealed class GremlinContractResolver : DefaultContractResolver
            {
                private readonly IGraphModel _model;

                public GremlinContractResolver(IGraphModel model)
                {
                    _model = model;
                }

                protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);

                    if ((_model.VerticesModel.TryGetMetadata(member) ?? _model.EdgesModel.TryGetMetadata(member)) is { Key.RawKey: string name })
                        property.PropertyName = name;

                    property.Readable = false;

                    return property;
                }
            }

            private sealed class JTokenConverterConverter : JsonConverter
            {
                private readonly IGremlinQueryEnvironment _environment;

                public JTokenConverterConverter(IGremlinQueryEnvironment environment)
                {
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

                public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) => throw new NotSupportedException($"Cannot write to {nameof(JTokenConverterConverter)}.");

                public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
                {
                    JToken? token;

                    if (reader is JTokenReader { CurrentToken: { } currentToken })
                    {
                        reader.Skip();
                        token = currentToken;
                    }
                    else
                        token = JToken.Load(reader);

                    try
                    {
                        _canConvert = false;

                        return _environment.Deserializer.TryTransformTo(objectType).From(token, _environment);
                    }
                    finally
                    {
                        _canConvert = true;
                    }
                }
            }

            [ThreadStatic]
            private static bool _canConvert;

            private static readonly ConditionalWeakTable<IGremlinQueryEnvironment, GraphsonJsonSerializer> Serializers = new();

            private GraphsonJsonSerializer(IGremlinQueryEnvironment environment)
            {
                ContractResolver = new GremlinContractResolver(environment.Model);
                Converters.Add(new JTokenConverterConverter(environment));
                // JTokenConverterConverter answers null for a value no converter can read, and the
                // serializer then fails to put that null where it does not fit. Such an entry of a
                // dictionary, or member of an object, is skipped, and the serializer carries on with the
                // next one. The event is raised again for every object around the failing one as the
                // error travels outwards; it is handled only where the failing entry or member is, so
                // that an object that cannot be read itself is still declined as a whole. A key that
                // cannot be read is skipped the same way. Only these failures are handled - any other
                // exception, one a converter of the caller's throws say, still takes the
                // deserialization down.
                Error += (_, args) =>
                {
                    if (args.ErrorContext.Error is JsonSerializationException or ArgumentNullException && args.CurrentObject is { } currentObject && ReferenceEquals(args.ErrorContext.OriginalObject, currentObject) && args.ErrorContext.Member is not null)
                    {
                        switch (ContractResolver.ResolveContract(currentObject.GetType()))
                        {
                            case JsonDictionaryContract:
                            {
                                // Declined, used to throw: ArgumentNullException from IDictionary.set_Item in JsonSerializerInternalReader.PopulateDictionary for { "a": 1, "b": null } as a Dictionary<string, int>.
                                args.ErrorContext.Handled = true;
                                break;
                            }
                            case JsonObjectContract:
                            {
                                // Used to decline the whole object: a JsonSerializationException from
                                // DynamicValueProvider.SetValue for { "Name": "x", "Age": null } as an object
                                // with an int Age. The member keeps the value the object was made with.
                                args.ErrorContext.Handled = true;
                                break;
                            }
                        }
                    }
                };
            }

            public static GraphsonJsonSerializer From(IGremlinQueryEnvironment environment) => Serializers.GetValue(
                environment,
                static environment => new GraphsonJsonSerializer(environment));

            public T? Deserialize<T>(JToken token)
            {
                _canConvert = false;

                return token.ToObject<T>(this);
            }
        }

        private sealed class NewtonsoftJsonSerializerConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            where TSource : JToken
        {
            // Every dictionary is a collection, and so is every collection interface the JObject of a
            // typed value implements itself - ICollection, IEnumerable. A JToken asked for as what it
            // is stays what it is.
            private static readonly bool TargetIsCollection = typeof(IEnumerable).IsAssignableFrom(typeof(TTarget)) && !typeof(JToken).IsAssignableFrom(typeof(TTarget));

            private readonly GraphsonJsonSerializer _serializer;

            public NewtonsoftJsonSerializerConverter(IGremlinQueryEnvironment environment)
            {
                _serializer = GraphsonJsonSerializer.From(environment);
            }

            bool IConverter<TSource, TTarget>.TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                // A typed value gets here when TypedValueConverter could not read its @value as the
                // type asked for. Asked for as a collection, the serializer would read its @type and
                // its @value as two entries, and the JObject would stand in for one as it is.
                if (TargetIsCollection && source is JObject jObject && jObject.IsTypedValueOtherThanMap())
                {
                    value = default;
                    return false;
                }

                if (source is TTarget alreadyRequestedValue)
                {
                    value = alreadyRequestedValue;
                    return true;
                }

                try
                {
                    if (_serializer.Deserialize<TTarget>(source) is { } requestedValue)
                    {
                        value = requestedValue;
                        return true;
                    }
                }
                // JsonException rather than JsonSerializationException: a token the requested type
                // cannot read - "not a date" for a DateTime - surfaces as a JsonReaderException,
                // and that has to make this converter decline instead of failing the whole result set.
                catch (JsonException)
                {

                }
                // Newtonsoft's reader converts a number or a string without catching what .NET throws
                // when it cannot, so these two arrive as they are.
                catch (OverflowException)
                {
                    // Declined, used to throw: OverflowException from BigInteger.op_Explicit in JsonReader.ReadAsInt32 for 9223372036854775808 as a byte or a short, and from Convert.ToByte in JsonReader.ReadArrayIntoByteArray for [ 300 ] or [ -1 ] as a byte[].
                }
                catch (FormatException)
                {
                    // Declined, used to throw: FormatException from Convert.FromBase64String in JsonReader.ReadAsBytes for "not base64!" as a byte[].
                }

                value = default;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            // Not for a property: PropertyConverterFactory reads one from an object and
            // ScalarToPropertyConverterFactory from a scalar, and what they decline stays declined.
            // The serializer would call the property's constructor with a null for a value it cannot
            // read, and that throws.
            // Declined, used to throw: ArgumentNullException from the constructor of Property<TValue> for { "key": "k", "value": null } as a Property<string>.
            //
            // Nor for a collection of key-value pairs, which KeyValuePairCollectionConverterFactory
            // reads as the dictionary it is. The serializer would read an array into a list of pairs,
            // and throw an ArgumentNullException for an item that is no pair.
            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && !typeof(Property).IsAssignableFrom(typeof(TTarget)) && !KeyValuePairCollectionConverterFactory.IsKeyValuePairCollection(typeof(TTarget), out _, out _)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NewtonsoftJsonSerializerConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget)), environment)
                : null;
        }
    }
}
