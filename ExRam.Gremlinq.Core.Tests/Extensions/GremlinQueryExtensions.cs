using ExRam.Gremlinq.Core.Serialization;
using FluentAssertions;
using FluentAssertions.Primitives;
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

            public SerializedGremlinQueryAssertions SerializeToGroovy(string serialization)
            {
                var pipeline = Subject.AsAdmin().Environment.Pipeline;
                var serializedQuery = pipeline.Serializer
                    .Serialize(Subject)
                    .As<GroovySerializedGremlinQuery>();
                    
                serializedQuery
                    .QueryString
                    .Should()
                    .Be(serialization);

                return new SerializedGremlinQueryAssertions(serializedQuery);
            }
        }

        public sealed class SerializedGremlinQueryAssertions : ObjectAssertions
        {
            private readonly GroovySerializedGremlinQuery _groovySerializedQuery;

            public SerializedGremlinQueryAssertions(GroovySerializedGremlinQuery groovySerializedQuery) : base(groovySerializedQuery)
            {
                _groovySerializedQuery = groovySerializedQuery;
            }

            public SerializedGremlinQueryAssertions WithParameters(params object[] parameters)
            {
                _groovySerializedQuery.Bindings.Should().HaveCount(parameters.Length);

                for (var i = 0; i < parameters.Length; i++)
                {
                    var label = i;
                    string key = null;

                    while (label > 0 || key == null)
                    {
                        key = (char)('a' + label % 26) + key;
                        label = label / 26;
                    }

                    key = "_" + key;
                    _groovySerializedQuery.Bindings.Should().Contain(key, parameters[i]);
                }

                return this;
            }

            public SerializedGremlinQueryAssertions WithoutParameters()
            {
                _groovySerializedQuery.Bindings.Should().BeEmpty();

                return this;
            }
        }

        public static GremlinQueryAssertions Should(this IGremlinQuery query)
        {
            return new GremlinQueryAssertions(query);
        }
    }
}
