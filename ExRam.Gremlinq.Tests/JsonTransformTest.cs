using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class JsonTransformTest
    {
        private readonly string[] _jsons =
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

        [Fact]
        public void Identity()
        {
            foreach (var json in _jsons)
            {
                AssertIdentity(JsonTransform.Identity(), json);
            }
        }

        [Fact]
        public void Identity_NestedValues()
        {
            var transform = JsonTransform.Identity().GraphElements().NestedValues();

            foreach (var json in _jsons)
            {
                AssertIdentity(transform, json);
            }

            Assert(
                transform,
                "{ \"@type\": \"g:Vertex\", \"@value\": { \"id\": 1 } }",
                "{ \"type\": \"vertex\", \"id\": 1 }");

            Assert(
                transform,
                "{ \"@type\": \"g:Vertex\", \"@value\": { \"id\": 1, \"properties\": { \"prop\": \"val\" } } }",
                "{ \"type\": \"vertex\", \"id\": 1, \"prop\": \"val\" }");

            foreach (var json in _jsons)
            {
                Assert(
                    transform,
                    $"{{ \"@type\": \"someType\", \"@value\": {json} }}",
                    json);
            }

            foreach (var json in _jsons)
            {
                Assert(
                    transform,
                    $"[ {{ \"@type\": \"someType\", \"@value\": {json} }} ]",
                    $"[{json}]");
            }

            foreach (var json in _jsons)
            {
                Assert(
                    transform,
                    $"{{ \"p\": 1, \"v\": {{ \"@type\": \"someType\", \"@value\": {json} }} }}",
                    $"{{ \"p\": 1, \"v\": {json} }}");
            }

            foreach (var json in _jsons)
            {
                Assert(
                    transform,
                    $"{{ \"v1\": {{ \"@type\": \"someType\", \"@value\": {json} }}, \"v2\": {{ \"@type\": \"someType\", \"@value\": {json} }} }}",
                    $"{{ \"v1\": {json}, \"v2\": {json} }}");
            }

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"prop\": \"val\" } }",
                "{ \"type\": \"vertex\", \"prop\": \"val\" }");

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"prop1\": \"val1\", \"prop2\": \"val2\" } }",
                "{ \"type\": \"vertex\", \"prop1\": \"val1\", \"prop2\": \"val2\" }");

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"prop1\": [1, 2, 3], \"prop2\": [3, 4, 5] } }",
                "{ \"type\": \"vertex\", \"prop1\": [1, 2, 3], \"prop2\": [3, 4, 5] }");

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"prop1\": [ { } ], \"prop2\": [3, 4, 5] } }",
                "{ \"type\": \"vertex\", \"prop1\": [ { } ], \"prop2\": [3, 4, 5] }");

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"name\": [ { \"@type\": \"g:VertexProperty\", \"@value\": 36 } ] } }",
                "{ \"type\": \"vertex\", \"name\": [ 36 ] }");

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"name\": [ { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": 36 } } ] } }",
                "{ \"type\": \"vertex\", \"name\": [ { \"id\": 36 } ] }");

            Assert(
                transform,
                "{ \"type\": \"vertex\", \"properties\": { \"name\": [ { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": 36 } } ] }, \"location\": \"wherever\" }",
                "{ \"type\": \"vertex\", \"name\": [ { \"id\": 36 } ], \"location\": \"wherever\" }");

            Assert(
                transform,
                "[ { \"@type\": \"g:Vertex\", \"@value\": { \"id\": { \"@type\": \"g:Int32\", \"@value\": 1 }, \"label\": \"person\", \"properties\": { \"name\": [ { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 0 }, \"value\": \"marko\", \"label\": \"name\" } } ], \"location\": [ { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 6 }, \"value\": \"san diego\", \"label\": \"location\", \"properties\": { \"startTime\": { \"@type\": \"g:Int32\", \"@value\": 1997 }, \"endTime\": { \"@type\": \"g:Int32\", \"@value\": 2001 } } } }, { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 7 }, \"value\": \"santa cruz\", \"label\": \"location\", \"properties\": { \"startTime\": { \"@type\": \"g:Int32\", \"@value\": 2001 }, \"endTime\": { \"@type\": \"g:Int32\", \"@value\": 2004 } } } }, { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 8 }, \"value\": \"brussels\", \"label\": \"location\", \"properties\": { \"startTime\": { \"@type\": \"g:Int32\", \"@value\": 2004 }, \"endTime\": { \"@type\": \"g:Int32\", \"@value\": 2005 } } } }, { \"@type\": \"g:VertexProperty\", \"@value\": { \"id\": { \"@type\": \"g:Int64\", \"@value\": 9 }, \"value\": \"santa fe\", \"label\": \"location\", \"properties\": { \"startTime\": { \"@type\": \"g:Int32\", \"@value\": 2005 } } } } ] } } }\r\n]",
                "[ { \"type\": \"vertex\", \"id\": 1, \"label\": \"person\", \"name\": [ { \"id\": 0, \"value\": \"marko\", \"label\": \"name\" } ], \"location\": [ { \"id\": 6, \"value\": \"san diego\", \"label\": \"location\", \"properties\": { \"startTime\": 1997, \"endTime\": 2001 } }, { \"id\": 7, \"value\": \"santa cruz\", \"label\": \"location\", \"properties\": { \"startTime\": 2001, \"endTime\": 2004 } }, { \"id\": 8, \"value\": \"brussels\", \"label\": \"location\", \"properties\": { \"startTime\": 2004, \"endTime\": 2005 } }, { \"id\": 9, \"value\": \"santa fe\", \"label\": \"location\", \"properties\": { \"startTime\": 2005 } } ] } ]");

            AssertIdentity(
                transform,
                "[ { \"value\": 1540202009475,\r\n    \"label\": \"Property1\",\r\n    \"properties\": {\r\n        \"metaKey\": \"MetaValue\"\r\n    }\r\n},\r\n{\r\n\"value\": \"Some string\",\r\n\"label\": \"Property2\"\r\n},\r\n{\r\n\"value\": 36,\r\n\"label\": \"Property3\"\r\n}\r\n]");
        }

        private static void AssertIdentity(IJsonTransform transform, string source)
        {
            Assert(transform, source, source);
        }

        private static void Assert(IJsonTransform transform, string source, string expected)
        {
            var transformed = JToken.Load(new JsonTextReader(new StringReader(source))
                .ToTokenEnumerable()
                .Apply(transform)
                .ToJsonReader());

            JToken.DeepEquals(JToken.Parse(expected), transformed).Should().BeTrue();
        }
    }
}

