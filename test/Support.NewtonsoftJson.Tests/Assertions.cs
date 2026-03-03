using FluentAssertions;

using Newtonsoft.Json;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class Assertions
    {
        public class VertexProperty
        {
            public string? Value { get; set; }
        }

        public class Vertex
        {
            public string? Id { get; set; }

            public VertexProperty? Property { get; set; }
        }

        [Fact]
        public void Empty_json_objects_are_deserialized_to_non_null_properties()
        {
            JsonConvert
                .DeserializeObject<Vertex>("{ \"id\": \"Hallo\" }")?
                .Property
                .Should()
                .BeNull();

            JsonConvert
                .DeserializeObject<Vertex>("{ \"id\": \"Hallo\" }")?
                .Id
                .Should()
                .NotBeNull();

            JsonConvert
                .DeserializeObject<Vertex>("{ \"id\": \"Hallo\", \"property\": { } }")?
                .Property
                .Should()
                .NotBeNull();
        }
    }
}
