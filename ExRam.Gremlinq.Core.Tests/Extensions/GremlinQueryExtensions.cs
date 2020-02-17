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
                    .Serialize(Subject)
                    .As<GroovySerializedGremlinQuery>();

                serializedQuery
                    .QueryString
                    .Should()
                    .Be(serialization);

                return new BindingsAssertions(serializedQuery.Bindings);
            }

            public BindingsAssertions SerializeToGraphson(string serialization)
            {
                var environment = Subject.AsAdmin().Environment;
                var serializedQuery = environment.Serializer
                    .Serialize(Subject);

                serializedQuery
                    .Should()
                    .BeOfType<Bytecode>();

                var bytecode = (Bytecode)serializedQuery;

                new GraphSON2Writer()
                    .WriteObject(bytecode)
                    .Should()
                    .Be(serialization);

                var bindings = new Dictionary<string, object>();

                void Collect(Bytecode bytecode)
                {
                    foreach(var instruction in bytecode.StepInstructions)
                    {
                        foreach(var argument in instruction.Arguments)
                        {
                            if (argument is Binding binding)
                                bindings[binding.Key] = binding.Value;
                            else if (argument is Bytecode subBytecode)
                                Collect(subBytecode);
                        }
                    }
                }

                Collect(bytecode);

                return new BindingsAssertions(bindings);
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
