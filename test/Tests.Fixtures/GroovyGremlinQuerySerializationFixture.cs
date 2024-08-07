using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class GroovyGremlinQuerySerializationFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
            .ConfigureEnvironment(env => env
                .ConfigureSerializer(ser => ser
                    .Add(ConverterFactory
                        .Create<Bytecode, RequestMessage>((bytecode, env, _, recurse) => recurse.TryTransform(bytecode, env, out GroovyGremlinScript groovyQuery) && recurse.TryTransform(groovyQuery, env, out RequestMessage? message)
                            ? message
                            : null))))
            .IgnoreCosmosDbSpecificProperties();
    }
}
