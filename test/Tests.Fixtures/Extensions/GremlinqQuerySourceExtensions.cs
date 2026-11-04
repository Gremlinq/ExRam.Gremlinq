using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Tests.Entities;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public static class GremlinqQuerySourceExtensions
    {
        public static IGremlinQuerySource IgnoreCosmosDbSpecificProperties(this IGremlinQuerySource source) => source
            .ConfigureEnvironment(env => env
                .ConfigureModel(model => model
                    .ConfigureElements(v => v
                        .ConfigureElement<Element>(conf => conf
                            .IgnoreAlways(p => p.PartitionKey)))));

        /// <summary>
        /// No element projection: a vertex, an edge or a vertex property comes back as the server returns it.
        /// </summary>
        public static IGremlinQuerySource WithoutElementProjection(this IGremlinQuerySource source) => source
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.VertexProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.EdgeProjectionSteps, Traversal.Empty)
                    .SetValue(GremlinqOption.VertexPropertyProjectionSteps, Traversal.Empty)));

        /// <summary>
        /// Sends a query as a Groovy script instead of bytecode: the request message is built from the Groovy form of
        /// the bytecode.
        /// </summary>
        public static IGremlinQuerySource SerializeToGroovy(this IGremlinQuerySource source) => source
            .ConfigureEnvironment(env => env
                .ConfigureSerializer(ser => ser
                    .Add(ConverterFactory
                        .Create<Bytecode, RequestMessage>((bytecode, env, _, recurse) => recurse.TryTransform(bytecode, env, out GroovyGremlinScript groovyQuery) && recurse.TryTransform(groovyQuery, env, out RequestMessage? message)
                            ? message
                            : null))));
    }
}
