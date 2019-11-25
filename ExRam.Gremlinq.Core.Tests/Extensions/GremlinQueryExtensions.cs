using System;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        public sealed class GremlinQueryAssertions : ReferenceTypeAssertions<IGremlinQuery, GremlinQueryAssertions>
        {
            public GremlinQueryAssertions(IGremlinQuery query)
            {
                Subject = query;
            }

            protected override string Identifier
            {
                get => typeof(IGremlinQuery).Name;
            }

            public SerializedGremlinQueryAssertions SerializeToGraphson(string serialization)
            {
                var pipeline = Subject.AsAdmin().Environment.Pipeline;
                var serializedQuery = new GraphSON2Writer().WriteObject(pipeline.Serializer
                    .Serialize(Subject));

                serializedQuery
                    .Should()
                    .Be(serialization);

                return new SerializedGremlinQueryAssertions(serializedQuery);
            }
        }

        public sealed class SerializedGremlinQueryAssertions : ObjectAssertions
        {
            private readonly string _groovySerializedQuery;

            public SerializedGremlinQueryAssertions(string groovySerializedQuery) : base(groovySerializedQuery)
            {
                _groovySerializedQuery = groovySerializedQuery;
            }

            public SerializedGremlinQueryAssertions WithParameters(params object[] parameters)
            {
                throw new NotImplementedException();
                /*_groovySerializedQuery.Bindings.Should().HaveCount(parameters.Length);

                for (var i = 0; i < parameters.Length; i++)
                {
                    var label = i;
                    string? key = null;

                    while (label > 0 || key == null)
                    {
                        key = (char)('a' + label % 26) + key;
                        label = label / 26;
                    }

                    key = "_" + key;

                    _groovySerializedQuery.Bindings.Should().ContainKey(key);
                    var value = _groovySerializedQuery.Bindings[key];

                    value.Should().BeEquivalentTo(parameters[i]);
                }*/

                return this;
            }

            public SerializedGremlinQueryAssertions WithoutParameters()
            {
                throw new NotImplementedException();

                //_groovySerializedQuery.Bindings.Should().BeEmpty();

                return this;
            }
        }

        public static GremlinQueryAssertions Should(this IGremlinQuery query)
        {
            return new GremlinQueryAssertions(query);
        }
    }
}
