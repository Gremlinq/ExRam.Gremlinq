using System.Dynamic;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class TransformerTest
    {
        private readonly IGremlinQueryEnvironment _environment;

        public TransformerTest()
        {
            _environment = GremlinQueryEnvironment.Invalid
                .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>())
                .UseNewtonsoftJson();
        }

        [Fact]
        public async Task Irrelevant() => await Verify(Transformer.Empty
            .Add(Create<JObject, string>((_, _, _, _) => "should not be here"))
            .TryTransformTo<string>().From("serialized", _environment));

        [Fact]
        public async Task More_specific_type_is_deserialized() => await Verify(_environment
            .Deserializer
            .TryTransformTo<object>().From(JObject.Parse("{ \"@type\": \"g:Date\", \"@value\": 1657527969000 }"), _environment));

        [Fact]
        public async Task JObject_is_not_changed()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = _environment
                .Deserializer
                .TryTransformTo<JObject>().From(original, _environment);

            deserialized
                .Should()
                .BeSameAs(original);
        }

        [Fact]
        public async Task Request_for_object_yields_DynamicObject()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = _environment
                .Deserializer
                .TryTransformTo<object>().From(original, _environment);

            deserialized
                .Should()
                .BeAssignableTo<DynamicObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_Dictionary()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = _environment
                .Deserializer
                .TryTransformTo<IDictionary<string, object>>().From(original, _environment);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_from_typed_GraphSON_yields_Dictionary()
        {
            var original = JObject.Parse("{ \"@type\": \"g:unknown\", \"@value\": { \"prop1\": \"value\", \"prop2\": 1657527969000 } }");

            var deserialized = _environment
                .Deserializer
                .TryTransformTo<IDictionary<string, object>>().From(original, _environment);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_object_from_map_yields_DynamicObject()
        {
            var original = JObject.Parse("{ \"@type\": \"g:Map\", \"@value\": [ \"name\", \"Daniel Weber\", \"timestamp\", { \"@type\": \"g:Date\", \"@value\": 1689868807115 } ] }");

            var deserialized = _environment
                .Deserializer
                .TryTransformTo<object>().From(original, _environment);

            deserialized
                .Should()
                .BeAssignableTo<DynamicObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Dynamic_access()
        {
            var original = JObject.Parse("{ \"@type\": \"g:Map\", \"@value\": [ \"name\", \"A name\", \"timestamp\", { \"@type\": \"g:Date\", \"@value\": 1689868807115 } ] }");

            var deserialized = _environment
                .Deserializer
                .TryTransformTo<dynamic>().From(original, _environment);

            var name = deserialized!.name;
            var timestamp = deserialized.timestamp;

            await Verify((name, timestamp));
        }

        [Fact]
        public async Task Overridden_request_for_Dictionary_yields_dictionary()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = _environment
                .Deserializer
                .Add(Create<JObject, IDictionary<string, object?>>((static (jObject, env, _, recurse) =>
                {
                    if (recurse.TryTransformTo<JObject>().From(jObject, env) is { } processedFragment)
                    {
                        var dict = new Dictionary<string, object?>();

                        foreach (var property in processedFragment)
                        {
                            dict.TryAdd(property.Key, recurse.TryTransformTo<object>().From(property.Value, env));
                        }

                        return dict;
                    }

                    return null;
                })))
                .TryTransformTo<IDictionary<string, object>>().From(original, _environment);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }

        [Fact]
        public Task Transform_to_List()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            return Verify(_environment
                .Deserializer
                .TransformTo<List<object>>()
                .From(token, _environment));
        }

        [Fact]
        public Task Transform_to_array()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            return Verify(_environment
                .Deserializer
                .TransformTo<object[]>()
                .From(token, _environment));
        }

        [Fact]
        public Task Transform_to_IEnumerable()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            var result = _environment
                .Deserializer
                .TransformTo<IEnumerable<object>>()
                .From(token, _environment);

            return Verify(result);
        }

        [Fact]
        public Task Transform_from_JArray_to_object()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            return Verify(_environment
                .Deserializer
                .TransformTo<object>()
                .From(token, _environment));
        }

        // A response is read with DateParseHandling.None, so no token of one holds a date. A
        // token that a caller read or built on its own may, and then the date it holds is the
        // date that is read, as it is held: the four tests below are what keeps these arms of
        // DateTimeConverterFactory and DateTimeOffsetConverterFactory under test.
        [Fact]
        public void DateTime_from_token_holding_DateTime()
        {
            var dateTime = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified);

            var actual = _environment
                .Deserializer
                .TryTransformTo<DateTime>().From(new JValue(dateTime), _environment);

            actual
                .Should()
                .Be(dateTime);

            actual!.Value.Kind
                .Should()
                .Be(DateTimeKind.Unspecified);
        }

        [Fact]
        public void DateTime_from_token_holding_DateTimeOffset()
        {
            var dateTimeOffset = new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.FromHours(2));

            var dateTime = _environment
                .Deserializer
                .TryTransformTo<DateTime>().From(new JValue(dateTimeOffset), _environment);

            dateTime
                .Should()
                .Be(new DateTime(2020, 1, 2, 1, 4, 5, DateTimeKind.Utc));

            dateTime!.Value.Kind
                .Should()
                .Be(DateTimeKind.Utc);
        }

        [Fact]
        public void DateTimeOffset_from_token_holding_DateTime()
        {
            _environment
                .Deserializer
                .TryTransformTo<DateTimeOffset>().From(new JValue(new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc)), _environment)
                .Should()
                .Be(new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero));
        }

        [Fact]
        public void DateTimeOffset_from_token_holding_DateTimeOffset()
        {
            var dateTimeOffset = new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.FromHours(2));

            var actual = _environment
                .Deserializer
                .TryTransformTo<DateTimeOffset>().From(new JValue(dateTimeOffset), _environment);

            actual
                .Should()
                .Be(dateTimeOffset);

            actual!.Value.Offset
                .Should()
                .Be(TimeSpan.FromHours(2));
        }
    }
}
