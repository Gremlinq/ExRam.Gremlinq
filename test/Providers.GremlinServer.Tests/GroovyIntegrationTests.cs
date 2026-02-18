using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public class GroovyIntegrationTests : QueryExecutionTest, IClassFixture<GroovyIntegrationTests.Fixture>
    {
        public class Fixture : CustomGremlinServerContainerFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => base
                .TransformQuerySource(container, g)
                .ConfigureEnvironment(env => env
                    .ConfigureSerializer(ser => ser
                        .Add(ConverterFactory
                            .Create<Bytecode, RequestMessage>((bytecode, env, _, recurse) => recurse.TryTransform(bytecode, env, out GroovyGremlinScript groovyQuery) && recurse.TryTransform(groovyQuery, env, out RequestMessage? message)
                                ? message
                                : null))));
        }

        public GroovyIntegrationTests(Fixture fixture) : base(
            fixture,
            new ExecutingVerifier())
        {
        }
    }
}
