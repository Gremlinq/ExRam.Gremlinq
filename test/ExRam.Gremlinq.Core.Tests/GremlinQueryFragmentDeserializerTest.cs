using System.Dynamic;
using ExRam.Gremlinq.Core.Deserialization;
using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryFragmentDeserializerTest : GremlinqTestBase
    {
        public GremlinQueryFragmentDeserializerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Empty()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<object>((serialized, type, env, overridden, recurse) => "overridden")
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<JObject>((serialized, type, env, overridden, recurse) => "should not be here")
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override1()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => overridden("overridden", type, env, recurse))
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override2()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => overridden("overridden 1", type, env, recurse))
                .Override<string>((serialized, type, env, overridden, recurse) => overridden("overridden 2", type, env, recurse))
                .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .TryDeserialize("serialized", typeof(int), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public void Recurse_wrong_type()
        {
            GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .Invoking(_ => _
                    .TryDeserialize("serialized", typeof(string), GremlinQueryEnvironment.Empty))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<int>((serialized, type, env, overridden, recurse) => overridden(37, type, env, recurse))
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .TryDeserialize("serialized", typeof(int), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .Override<string>((serialized, type, env, overridden, recurse) => recurse.TryDeserialize(36, type, env))
                .Override<int>((serialized, type, env, overridden, recurse) => overridden(37, type, env, recurse))
                .TryDeserialize("serialized", typeof(int), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task More_specific_type_is_deserialized()
        {
            await Verify(GremlinQueryFragmentDeserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize(JObject.Parse("{ \"@type\": \"g:Date\", \"@value\": 1657527969000 }") , typeof(object), GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task JObject_is_not_changed()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryFragmentDeserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize(original, typeof(JObject), GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeSameAs(original);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_expandoObject()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryFragmentDeserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize(original, typeof(IDictionary<string, object>), GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<ExpandoObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_expandoObject_from_typed_GraphSON()
        {
            var original = JObject.Parse("{ \"@type\": \"g:unknown\", \"@value\": { \"prop1\": \"value\", \"prop2\": 1657527969000 } }");

            var deserialized = GremlinQueryFragmentDeserializer.Identity
                .AddNewtonsoftJson()
                .TryDeserialize(original, typeof(IDictionary<string, object>), GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<ExpandoObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Overridden_request_for_Dictionary_yields_dictionary()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryFragmentDeserializer.Identity
                .AddNewtonsoftJson()
                .Override<JObject, IDictionary<string, object?>>(static (jObject, type, env, overridden, recurse) =>
                {
                    if (recurse.TryDeserialize(jObject, typeof(JObject), env) is JObject processedFragment)
                    {
                        var dict = new Dictionary<string, object?>();

                        foreach (var property in processedFragment)
                        {
                            dict.TryAdd(property.Key, recurse.TryDeserialize(property.Value, typeof(object), env));
                        }

                        return dict;
                    }

                    return overridden(jObject, type, env, recurse);
                })
                .TryDeserialize(original, typeof(IDictionary<string, object>), GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<Dictionary<string, object?>>();

            await Verify(deserialized);
        }
    }
}
