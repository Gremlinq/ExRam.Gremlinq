using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class JsonTransformTest
    {
        [Fact]
        public void Identity_Test()
        {
            var jsons = new[]
            {
                "36",

                "{ }",
                "{ \"key1\": \"value1\" }",
                "{ \"key1\": \"value1\", \"key2\": \"value2\" }",
                "{ \"key1\": { \"key2\": \"value2\" } }",
                "{ \"key1\": { \"key2\": \"value2\" }, \"key3\": { \"key4\": \"value4\" } }",

                "[ ]",
                "[ 36, 37 ]",
                "[ { \"key1\": \"value1\" } ] ",
                "[ 36, { \"key1\": \"value1\" } ] ",
                "[ { \"key1\": \"value1\", \"key2\": \"value2\" } ] ",
                "[ 36, { \"key1\": \"value1\", \"key2\": \"value2\" } ] ",
                "[ { \"key1\": \"value1\" }, { \"key1\": \"value1\", \"key2\": \"value2\" } ] ",
                "[ { \"key1\": { \"key2\": \"value2\" }, \"key3\": { \"key4\": \"value4\" } } ]",
                "[ 36, { \"key1\": \"value1\" }, { \"key1\": \"value1\", \"key2\": \"value2\" } ] ",

                "[ [ 36 ], [ 37 ] ]",
                "[ [ { \"key1\": \"value1\" } ] ] ",
                "[ [ 36 ], [ { \"key1\": \"value1\" } ] ] ",
                "[ [ { \"key1\": \"value1\", \"key2\": \"value2\" } ] ] ",
                "[ [ 36 ], [ { \"key1\": \"value1\", \"key2\": \"value2\" } ] ] ",
                "[ [ { \"key1\": \"value1\" } ], [ { \"key1\": \"value1\", \"key2\": \"value2\" } ] ] ",
                "[ [ 36 ], [ { \"key1\": \"value1\" } ], [ { \"key1\": \"value1\", \"key2\": \"value2\" } ] ] "
            };

            foreach (var json in jsons)
            {
                var token = JToken.Parse(json);
                var transformed = token.Transform(JsonTransformRules.Identity).IfNone(JValue.CreateNull());

                JToken.DeepEquals(token, transformed).Should().BeTrue();
            }
        }

        [Fact]
        public void Deserializing_arrays_from_empty_JArray_yields_empty_array()
        {
            new GraphsonDeserializer().Deserialize<int[]>(new JTokenReader(new JArray())).Should().BeEmpty();
            new GraphsonDeserializer().Deserialize<string[]>(new JTokenReader(new JArray())).Should().BeEmpty();
            new GraphsonDeserializer().Deserialize<object[]>(new JTokenReader(new JArray())).Should().BeEmpty();
        }
    }
}