using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class JTokenHeuristicTests
    {
        private readonly IGremlinQueryEnvironment _environment;

        public JTokenHeuristicTests()
        {
            _environment = GremlinQueryEnvironment.Invalid
                .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>())
                .UseNewtonsoftJson();
        }

        [Fact]
        public void LooksLikeElement_with_id_and_label()
        {
            var jObject = JObject.Parse("""{ "id": 1, "label": "person" }""");

            jObject.LooksLikeElement(out var idToken, out var labelValue, out var propertiesObject)
                .Should().BeTrue();

            idToken.Should().NotBeNull();
            labelValue!.Value<string>().Should().Be("person");
            propertiesObject.Should().BeNull();
        }

        [Fact]
        public void LooksLikeElement_with_properties()
        {
            var jObject = JObject.Parse("""{ "id": 1, "label": "person", "properties": { "name": "test" } }""");

            jObject.LooksLikeElement(out _, out _, out var propertiesObject)
                .Should().BeTrue();

            propertiesObject.Should().NotBeNull();
        }

        [Fact]
        public void LooksLikeElement_without_id_returns_false()
        {
            var jObject = JObject.Parse("""{ "label": "person" }""");

            jObject.LooksLikeElement(out _, out _, out _)
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeElement_without_label_returns_false()
        {
            var jObject = JObject.Parse("""{ "id": 1 }""");

            jObject.LooksLikeElement(out _, out _, out _)
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeElement_with_value_key_returns_false()
        {
            var jObject = JObject.Parse("""{ "id": 1, "label": "person", "value": "something" }""");

            jObject.LooksLikeElement(out _, out _, out _)
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeElement_with_array_id_returns_false()
        {
            var jObject = JObject.Parse("""{ "id": [1, 2], "label": "person" }""");

            jObject.LooksLikeElement(out _, out _, out _)
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeElement_with_non_string_label_returns_false()
        {
            var jObject = JObject.Parse("""{ "id": 1, "label": 123 }""");

            jObject.LooksLikeElement(out _, out _, out _)
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeElement_with_non_object_properties_returns_false()
        {
            var jObject = JObject.Parse("""{ "id": 1, "label": "person", "properties": "not-object" }""");

            jObject.LooksLikeElement(out _, out _, out _)
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeProperty_with_value_and_key()
        {
            var jObject = JObject.Parse("""{ "value": "test", "key": "name" }""");

            jObject.LooksLikeProperty()
                .Should().BeTrue();
        }

        [Fact]
        public void LooksLikeProperty_without_value_returns_false()
        {
            var jObject = JObject.Parse("""{ "key": "name" }""");

            jObject.LooksLikeProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeProperty_without_key_returns_false()
        {
            var jObject = JObject.Parse("""{ "value": "test" }""");

            jObject.LooksLikeProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeProperty_with_non_string_key_returns_false()
        {
            var jObject = JObject.Parse("""{ "value": "test", "key": 123 }""");

            jObject.LooksLikeProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeVertexProperty_with_value_and_id()
        {
            var jObject = JObject.Parse("""{ "value": "test", "id": 1 }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeTrue();
        }

        [Fact]
        public void LooksLikeVertexProperty_with_label_and_properties()
        {
            var jObject = JObject.Parse("""{ "value": "test", "id": 1, "label": "name", "properties": { "p": 1 } }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeTrue();
        }

        [Fact]
        public void LooksLikeVertexProperty_without_value_returns_false()
        {
            var jObject = JObject.Parse("""{ "id": 1 }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeVertexProperty_without_id_returns_false()
        {
            var jObject = JObject.Parse("""{ "value": "test" }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeVertexProperty_with_array_id_returns_false()
        {
            var jObject = JObject.Parse("""{ "value": "test", "id": [1, 2] }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeVertexProperty_with_non_string_label_returns_false()
        {
            var jObject = JObject.Parse("""{ "value": "test", "id": 1, "label": 123 }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void LooksLikeVertexProperty_with_non_object_properties_returns_false()
        {
            var jObject = JObject.Parse("""{ "value": "test", "id": 1, "properties": "not-object" }""");

            jObject.LooksLikeVertexProperty()
                .Should().BeFalse();
        }

        [Fact]
        public void TryExpandTraverser_non_traverser_returns_null()
        {
            var jObject = JObject.Parse("""{ "name": "test" }""");

            jObject.TryExpandTraverser<object>(_environment, _environment.Deserializer)
                .Should().BeNull();
        }

        [Fact]
        public void TryExpandTraverser_valid_traverser()
        {
            var jObject = JObject.Parse("""{ "@type": "g:Traverser", "@value": { "bulk": { "@type": "g:Int64", "@value": 2 }, "value": "hello" } }""");

            var result = jObject.TryExpandTraverser<object>(_environment, _environment.Deserializer);

            result.Should().NotBeNull();
            result!.Should().HaveCount(2);
        }

        [Fact]
        public void TryExpandTraverser_null_value_yields_defaults()
        {
            var jObject = JObject.Parse("""{ "@type": "g:Traverser", "@value": { "bulk": { "@type": "g:Int64", "@value": 1 }, "value": null } }""");

            var result = jObject.TryExpandTraverser<string>(_environment, _environment.Deserializer);

            result.Should().NotBeNull();
            result!.Should().ContainSingle().Which.Should().BeNull();
        }
    }
}
