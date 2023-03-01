using System.Dynamic;
using ExRam.Gremlinq.Core.Deserialization;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public class DeserializerTest : GremlinqTestBase
    {
        public DeserializerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Empty()
        {
            await Verify(Deserializer.Identity
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verify(Deserializer.Identity
                .Override<object, string>((serialized, env, recurse) => "overridden")
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verify(Deserializer.Identity
                .Override<JObject, string>((serialized, env, recurse) => "should not be here")
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override1()
        {
            await Verify(Deserializer.Identity
                .Override<string, string>((serialized, env, recurse) => "overridden 1")
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override2()
        {
            await Verify(Deserializer.Identity
                .Override<string, string>((serialized, env, recurse) => "overridden 1")
                .Override<string, string>((serialized, env, recurse) => "overridden 2")
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verify(Deserializer.Identity
                .Override<string, int>((serialized, env, recurse) => recurse.TryDeserialize<int>().From(36, env))
                .TryDeserialize<int>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public void Recurse_wrong_type()
        {
            Deserializer.Identity
                .Override<string, int>((serialized, env, recurse) => recurse.TryDeserialize<int>().From(36, env))
                .TryTransform<int, string>(36, GremlinQueryEnvironment.Empty, out var _)
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verify(Deserializer.Identity
                .Override<int, string>((serialized, env, recurse) => serialized.ToString())
                .Override<string, string>((serialized, env, recurse) => recurse.TryDeserialize<string>().From(serialized.Length, env))
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verify(Deserializer.Identity
                .Override<string, string>((serialized, env, recurse) => recurse.TryDeserialize<string>().From(serialized.Length, env))
                .Override<int, string>((serialized, env, recurse) => serialized.ToString())
                .TryDeserialize<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task More_specific_type_is_deserialized()
        {
            await Verify(Deserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize<object>().From(JObject.Parse("{ \"@type\": \"g:Date\", \"@value\": 1657527969000 }"), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task JObject_is_not_changed()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = Deserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize<JObject>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeSameAs(original);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_expandoObject()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = Deserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<ExpandoObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_expandoObject_from_typed_GraphSON()
        {
            var original = JObject.Parse("{ \"@type\": \"g:unknown\", \"@value\": { \"prop1\": \"value\", \"prop2\": 1657527969000 } }");

            var deserialized = Deserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<ExpandoObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Overridden_request_for_Dictionary_yields_dictionary()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = Deserializer.Identity
                .AddNewtonsoftJson()
                .Override<JObject, IDictionary<string, object?>>(static (jObject,  env, recurse) =>
                {
                    if (recurse.TryDeserialize<JObject>().From(jObject, env) is JObject processedFragment)
                    {
                        var dict = new Dictionary<string, object?>();

                        foreach (var property in processedFragment)
                        {
                            dict.TryAdd(property.Key, recurse.TryDeserialize<object>().From(property.Value, env));
                        }

                        return dict;
                    }

                    return default;
                })
                .TryDeserialize<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }
    }
}
