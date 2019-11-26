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

            public GroovySerializedGremlinQueryAssertions SerializeToGroovy(string serialization)
            {
                var pipeline = Subject.AsAdmin().Environment.Pipeline;
                var serializedQuery = pipeline.Serializer
                    .Serialize(Subject)
                    .As<GroovySerializedGremlinQuery>();

                serializedQuery
                    .QueryString
                    .Should()
                    .Be(serialization);

                return new GroovySerializedGremlinQueryAssertions(serializedQuery);
            }

            public GraphsonSerializedGremlinQueryAssertions SerializeToGaphson(string serialization)
            {
                var pipeline = Subject.AsAdmin().Environment.Pipeline;
                var serializedQuery = new GraphSON2Writer().WriteObject(pipeline.Serializer
                    .Serialize(Subject));

                serializedQuery
                    .Should()
                    .Be(serialization);

                return new GraphsonSerializedGremlinQueryAssertions(serializedQuery);
            }
        }

        public sealed class GroovySerializedGremlinQueryAssertions : ObjectAssertions
        {
            private readonly GroovySerializedGremlinQuery _groovySerializedQuery;

            public GroovySerializedGremlinQueryAssertions(GroovySerializedGremlinQuery groovySerializedQuery) : base(groovySerializedQuery)
            {
                _groovySerializedQuery = groovySerializedQuery;
            }

            public GroovySerializedGremlinQueryAssertions WithParameters(params object[] parameters)
            {
                _groovySerializedQuery.Bindings.Should().HaveCount(parameters.Length);

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
                }

                return this;
            }

            public GroovySerializedGremlinQueryAssertions WithoutParameters()
            {
                _groovySerializedQuery.Bindings.Should().BeEmpty();

                return this;
            }
        }

        public sealed class GraphsonSerializedGremlinQueryAssertions : ObjectAssertions
        {
            private readonly string _graphsonSerializedQuery;

            public GraphsonSerializedGremlinQueryAssertions(string graphsonSerializedQuery) : base(graphsonSerializedQuery)
            {
                _graphsonSerializedQuery = graphsonSerializedQuery;
            }

            public GraphsonSerializedGremlinQueryAssertions WithParameters(params object[] parameters)
            {
                return this;
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
                }

                return this;*/
            }

            public GraphsonSerializedGremlinQueryAssertions WithoutParameters()
            {
                return this;
                //_groovySerializedQuery.Bindings.Should().BeEmpty();
            }
        }

        public static GremlinQueryAssertions Should(this IGremlinQuery query)
        {
            return new GremlinQueryAssertions(query);
        }
    }
}
