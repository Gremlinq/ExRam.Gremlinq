using System.Collections.Generic;
using ExRam.Gremlinq.Providers.CosmosDb;
using ExRam.Gremlinq.Core.Serialization;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace ExRam.Gremlinq.Core.Tests
{
    internal static class GremlinQueryExtensions
    {
        public sealed class GremlinQueryAssertions : ReferenceTypeAssertions<IGremlinQuery, GremlinQueryAssertions>
        {
            private static readonly StringGremlinQuerySerializer<CosmosDbGroovyGremlinQueryElementVisitor> Serializer = new StringGremlinQuerySerializer<CosmosDbGroovyGremlinQueryElementVisitor>();

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
                var tuple = Serializer.Serialize(Subject);

                tuple.queryString
                    .Should()
                    .Be(serialization);

                return new SerializedGremlinQueryAssertions(tuple);
            }
        }

        public sealed class SerializedGremlinQueryAssertions : ObjectAssertions
        {
            private readonly (string queryString, IDictionary<string, object> parameters) _tuple;

            public SerializedGremlinQueryAssertions((string queryString, IDictionary<string, object> parameters) tuple) : base(tuple)
            {
                _tuple = tuple;
            }

            public SerializedGremlinQueryAssertions WithParameters(params object[] parameters)
            {
                _tuple.parameters.Should().HaveCount(parameters.Length);

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
                    _tuple.parameters.Should().Contain(key, parameters[i]);
                }

                return this;
            }

            public SerializedGremlinQueryAssertions WithoutParameters()
            {
                _tuple.parameters.Should().BeEmpty();

                return this;
            }
        }

        public static GremlinQueryAssertions Should(this IGremlinQuery query)
        {
            return new GremlinQueryAssertions(query);
        }
    }
}
