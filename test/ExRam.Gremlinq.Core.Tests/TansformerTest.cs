using System.Dynamic;
using ExRam.Gremlinq.Core.Transformation;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TansformerTest : GremlinqTestBase
    {
        public TansformerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Empty()
        {
            await Verify(Transformer.Identity
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type()
        {
            await Verify(Transformer.Identity
                .Add(Create<object, string>((serialized, env, recurse) => "overridden"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type_in_request()
        {
            await Verify(Transformer.Identity
                .Add(Create<object, string>((serialized, env, recurse) => "overridden"))
                .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Base_type_in_request_with_struct_available()
        {
            await Verify(Transformer.Identity
                .Add(Create<object, int>((serialized, env, recurse) => 36))
                .TryTransformTo<object>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Irrelevant()
        {
            await Verify(Transformer.Identity
                .Add(Create<JObject, string>((serialized, env, recurse) => "should not be here"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override1()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, string>((serialized, env, recurse) => "overridden 1"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Override2()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, string>((serialized, env, recurse) => "overridden 1"))
                .Add(Create<string, string>((serialized, env, recurse) => "overridden 2"))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, int>((serialized, env, recurse) => recurse.TryTransformTo<int>().From(36, env)))
                .TryTransformTo<int>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public void Recurse_wrong_type()
        {
            Transformer.Identity
                .Add(Create<string, int>((serialized, env, recurse) => recurse.TryTransformTo<int>().From(36, env)))
                .TryTransform<int, string>(36, GremlinQueryEnvironment.Empty, out var _)
                .Should()
                .BeFalse();
        }

        [Fact]
        public async Task Recurse_to_previous_override()
        {
            await Verify(Transformer.Identity
                .Add(Create<int, string>((serialized, env, recurse) => serialized.ToString()))
                .Add(Create<string, string>((serialized, env, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
                .TryTransformTo<string>().From("serialized", GremlinQueryEnvironment.Empty));
        }

        [Fact]
        public async Task Recurse_to_later_override()
        {
            await Verify(Transformer.Identity
                .Add(Create<string, string>((serialized, env, recurse) => recurse.TryTransformTo<string>().From(serialized.Length, env)))
                .Add(Create<int, string>((serialized, env, recurse) => serialized.ToString()))
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
        public async Task Request_for_Dictionary_yields_expandoObject()
        {
            var original = JObject.Parse("{ \"prop1\": \"value\", \"prop2\": 1657527969000 }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<ExpandoObject>();

            await Verify(deserialized);
        }

        [Fact]
        public async Task Request_for_Dictionary_yields_expandoObject_from_typed_GraphSON()
        {
            var original = JObject.Parse("{ \"@type\": \"g:unknown\", \"@value\": { \"prop1\": \"value\", \"prop2\": 1657527969000 } }");

            var deserialized = GremlinQueryEnvironment.Default
                .UseNewtonsoftJson()
                .Deserializer
                .TryTransformTo<IDictionary<string, object>>().From(original, GremlinQueryEnvironment.Empty);

            deserialized
                .Should()
                .BeOfType<ExpandoObject>();

            await Verify(deserialized);
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
    }
}
