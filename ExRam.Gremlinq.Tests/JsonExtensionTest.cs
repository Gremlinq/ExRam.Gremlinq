using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class JsonExtensionTest
    {
        [Fact]
        public void Empty_Array_Identity_Projection()
        {
            var tokenArray = new JTokenReader(JToken.Parse("[]"))
                .ToTokenEnumerable()
                .Apply(x => x.SelectArray(y => y))
                .ToArray();

            tokenArray.Should().HaveCount(2);
            tokenArray[0].tokenType.Should().Be(JsonToken.StartArray);
            tokenArray[1].tokenType.Should().Be(JsonToken.EndArray);
        }

        [Fact]
        public void String_Array_Identity_Projection()
        {
            var tokenArray = new JTokenReader(JToken.Parse("[ \"String1\", \"String2\" ]"))
                .ToTokenEnumerable()
                .Apply(x => x.SelectArray(y => y))
                .ToArray();

            tokenArray.Should().HaveCount(4);

            tokenArray[0].tokenType.Should().Be(JsonToken.StartArray);

            tokenArray[1].tokenType.Should().Be(JsonToken.String);
            tokenArray[1].tokenValue.Should().Be("String1");

            tokenArray[2].tokenType.Should().Be(JsonToken.String);
            tokenArray[2].tokenValue.Should().Be("String2");

            tokenArray[3].tokenType.Should().Be(JsonToken.EndArray);
        }

        [Fact]
        public void Object_Array_Identity_Projection1()
        {
            var tokenArray = new JTokenReader(JToken.Parse("[ { }, { } ]"))
                .ToTokenEnumerable()
                .Apply(x => x.SelectArray(y => y))
                .ToArray();

            tokenArray.Should().HaveCount(6);

            tokenArray[0].tokenType.Should().Be(JsonToken.StartArray);

            tokenArray[1].tokenType.Should().Be(JsonToken.StartObject);
            tokenArray[2].tokenType.Should().Be(JsonToken.EndObject);

            tokenArray[3].tokenType.Should().Be(JsonToken.StartObject);
            tokenArray[4].tokenType.Should().Be(JsonToken.EndObject);

            tokenArray[5].tokenType.Should().Be(JsonToken.EndArray);
        }

        [Fact]
        public void Object_array_property_extraction()
        {
            var tokenArray = new JTokenReader(JToken.Parse("[ { \"Key\": \"Value1\" }, { \"Key\": \"Value2\" } ]"))
                .ToTokenEnumerable()
                .Apply(x => x.SelectArray(y => y.ExtractProperty("Key")))
                .ToArray();

            tokenArray.Should().HaveCount(4);

            tokenArray[0].tokenType.Should().Be(JsonToken.StartArray);

            tokenArray[1].tokenType.Should().Be(JsonToken.String);
            tokenArray[1].tokenValue.Should().Be("Value1");

            tokenArray[2].tokenType.Should().Be(JsonToken.String);
            tokenArray[2].tokenValue.Should().Be("Value2");

            tokenArray[3].tokenType.Should().Be(JsonToken.EndArray);
        }

        [Fact]
        public void Single_object_property_extraction()
        {
            var tokenArray = new JTokenReader(JToken.Parse("{ \"Key\": \"Value\" }"))
                .ToTokenEnumerable()
                .Apply(x => x.ExtractProperty("Key"))
                .ToArray();

            tokenArray.Should().HaveCount(1);

            tokenArray[0].tokenType.Should().Be(JsonToken.String);
        }

        [Fact]
        public void SelectArray_on_object()
        {
            var tokenArray = new JTokenReader(JToken.Parse("{ \"Key\": \"Value\" }"))
                .ToTokenEnumerable()
                .Apply(x => x.SelectArray(_ => _))
                .ToArray();

            tokenArray.Should().HaveCount(4);

            tokenArray[0].tokenType.Should().Be(JsonToken.StartObject);

            tokenArray[1].tokenType.Should().Be(JsonToken.PropertyName);
            tokenArray[1].tokenValue.Should().Be("Key");

            tokenArray[2].tokenType.Should().Be(JsonToken.String);
            tokenArray[2].tokenValue.Should().Be("Value");

            tokenArray[3].tokenType.Should().Be(JsonToken.EndObject);
        }

        [Fact]
        public void Unwrap()
        {
            var tokenArray = new JTokenReader(JToken.Parse("Properties: { \"Key\": \"Value\" }"))
                .ToTokenEnumerable()
                .Apply(x => x.UnwrapObject("Properties" _ => _))
                .ToArray();

        }
    }
}