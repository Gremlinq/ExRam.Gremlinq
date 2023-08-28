using System.Dynamic;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TransformerTest : VerifyBase
    {
        public TransformerTest() : base(GremlinQueryVerifier.DefaultSettings)
        {

        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verify(Transformer.Identity
                .Add(Create<JObject, string>((serialized, env, recurse) => "should not be here"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task More_specific_type_is_deserialized()
        {
            await Verify(GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<object>().From(JObject.Parse("{ \"@type\": \"g:Date\", \"@value\": 1657527969000 }"), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task JObject_is_not_changed()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<JObject>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeSameAs(original);
        }

        [Fact]
        public async Task Request_for_object_yields_DynamicObject()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<object>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeAssignableTo<DynamicObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_Dictionary()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_from_typed_GraphSON_yields_Dictionary()
        {
            var original = JObject.Parse("{ \"@type\": \"g:unknown\", \"@value\": { \"prop1\": \"value\", \"prop2\": 1657527969000 } }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_object_from_map_yields_DynamicObject()
        {
            var original = JObject.Parse("{ \"@type\": \"g:Map\", \"@value\": [ \"name\", \"Daniel Weber\", \"timestamp\", { \"@type\": \"g:Date\", \"@value\": 1689868807115 } ] }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<object>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeAssignableTo<DynamicObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Dynamic_access()
        {
            var original = JObject.Parse("{ \"@type\": \"g:Map\", \"@value\": [ \"name\", \"A name\", \"timestamp\", { \"@type\": \"g:Date\", \"@value\": 1689868807115 } ] }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<dynamic>().From(original, GremlinQueryEnvironment.Empty);

            var name = deserialized!.name;
            var timestamp = deserialized!.timestamp;

            await Verify((name, timestamp));
        }

        [Fact]
        public async Task Overridden_request_for_Dictionary_yields_dictionary()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .Add(Create<JObject, IDictionary<string, object?>>((static (jObject,  env, recurse) =>
                {
                    if (recurse.TryTransformTo<JObject>().From(jObject, env) is JObject processedFragment)
                    {
                        var dict = new Dictionary<string, object?>();

                        foreach (var property in processedFragment)
                        {
                            dict.TryAdd(property.Key, recurse.TryTransformTo<object>().From(property.Value, env));
                        }

                        return dict;
                    }

                    return default;
                })))
                .TryTransformTo<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }

        [Fact]
        public Task Transform_to_List()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            return Verify(GremlinQueryEnvironment.Empty
                .UseNewtonsoftJson()
                .Deserializer
                .TransformTo<List<object>>()
                .From(token, GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public Task Transform_to_array()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            return Verify(GremlinQueryEnvironment.Empty
                .UseNewtonsoftJson()
                .Deserializer
                .TransformTo<object[]>()
                .From(token, GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public Task Transform_to_IEnumerable()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            var result = GremlinQueryEnvironment.Empty
                .UseNewtonsoftJson()
                .Deserializer
                .TransformTo<IEnumerable<object>>()
                .From(token, GremlinQueryEnvironment.Empty);

            return Verify(result);
        }

        [Fact]
        public Task Transform_from_JArray_to_object()
        {
            var token = JObject.Parse("{ \"@type\": \"g:List\", \"@value\": [ { \"@type\": \"g:Traverser\", \"@value\": { \"bulk\": { \"@type\": \"g:Int64\", \"@value\": 3 }, \"value\": { \"@type\": \"g:Map\", \"@value\": [ \"id\", { \"@type\": \"g:Int64\", \"@value\": 184 }, \"label\", \"Label\", \"properties\", { \"@type\": \"g:Map\", \"@value\": [] } ] } } } ]}");

            return Verify(GremlinQueryEnvironment.Empty
                .UseNewtonsoftJson()
                .Deserializer
                .TransformTo<object>()
                .From(token, GremlinQueryEnvironment.Empty));
        }
    }
}
