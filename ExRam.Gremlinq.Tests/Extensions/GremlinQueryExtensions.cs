using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace ExRam.Gremlinq.Tests
{
    internal static class GremlinQueryExtensions
    {
        public sealed class GremlinQueryAssertions : ReferenceTypeAssertions<IGremlinQuery, GremlinQueryAssertions>
        {
            private readonly IGremlinQuery _query;

            public GremlinQueryAssertions(IGremlinQuery query)
            {
                _query = query;
            }

            protected override string Identifier
            {
                get => typeof(IGremlinQuery).Name;
            }

            public SerializedGremlinQueryAssertions SerializeTo(string serialization)
            {
                var tuple = _query.Serialize();

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
                    _tuple.parameters.Should().Contain($"_P{i + 1}", parameters[i]);
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
