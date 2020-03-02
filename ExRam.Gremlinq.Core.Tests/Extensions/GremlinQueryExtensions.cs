using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        public sealed class GremlinQueryAssertions : ReferenceTypeAssertions<IGremlinQueryBase, GremlinQueryAssertions>
        {
            public GremlinQueryAssertions(IGremlinQueryBase query)
            {
                Subject = query;
            }

            protected override string Identifier
            {
                get => typeof(IGremlinQueryBase).Name;
            }

            public BindingsAssertions SerializeToGroovy(string serialization)
            {
                var environment = Subject.AsAdmin().Environment;
                var serializedQuery = environment.Serializer
                    .Serialize(Subject);
                
                if (serializedQuery is Bytecode bytecode)
                    serializedQuery = bytecode.ToGroovy();

                var groovy = serializedQuery
                    .Should()
                    .BeOfType<GroovyScript>()
                    .Subject;

                groovy
                    .QueryString
                    .Should()
                    .Be(serialization);

                return new BindingsAssertions(groovy.Bindings);
            }

            public GremlinQueryAssertions SerializeToGraphson(string serialization)
            {
                var environment = Subject.AsAdmin().Environment;
                var serializedQuery = environment.Serializer
                    .Serialize(Subject);

                var bytecode = serializedQuery
                    .Should()
                    .BeOfType<Bytecode>()
                    .Subject;

                new GraphSON2Writer()
                    .WriteObject(bytecode)
                    .Should()
                    .Be(serialization);

                return this;
            }
        }

        public sealed class BindingsAssertions : ObjectAssertions
        {
            private readonly Dictionary<string, object> _bindings;

            public BindingsAssertions(Dictionary<string, object> bindings) : base(bindings)
            {
                _bindings = bindings;
            }

            public BindingsAssertions WithParameters(params object[] parameters)
            {
                _bindings.Should().HaveCount(parameters.Length);

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

                    _bindings.Should().ContainKey(key);
                    var value = _bindings[key];

                    value.Should().BeEquivalentTo(parameters[i]);
                }

                return this;
            }

            public BindingsAssertions WithoutParameters()
            {
                _bindings.Should().BeEmpty();

                return this;
            }
        }

        public static GremlinQueryAssertions Should(this IGremlinQueryBase query)
        {
            return new GremlinQueryAssertions(query);
        }
    }
}
