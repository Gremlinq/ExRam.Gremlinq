using System.Reflection;
using System.Runtime.CompilerServices;

using PublicApiGenerator;

namespace ExRam.Gremlinq.PublicApi.Tests
{
    public class PublicApiTests
    {
        [Fact]
        public Task Core() => Verify("ExRam.Gremlinq.Core");

        [Fact]
        public Task Core_AspNet() => Verify("ExRam.Gremlinq.Core.AspNet");

        [Fact]
        public Task Providers_Core() => Verify("ExRam.Gremlinq.Providers.Core");

        [Fact]
        public Task Providers_CosmosDb() => Verify("ExRam.Gremlinq.Providers.CosmosDb");

        [Fact]
        public Task Providers_CosmosDb_AspNet() => Verify("ExRam.Gremlinq.Providers.CosmosDb.AspNet");

        [Fact]
        public Task Providers_GremlinServer() => Verify("ExRam.Gremlinq.Providers.GremlinServer");

        [Fact]
        public Task Providers_GremlinServer_AspNet() => Verify("ExRam.Gremlinq.Providers.GremlinServer.AspNet");

        [Fact]
        public Task Providers_JanusGraph() => Verify("ExRam.Gremlinq.Providers.JanusGraph");

        [Fact]
        public Task Providers_JanusGraph_AspNet() => Verify("ExRam.Gremlinq.Providers.JanusGraph.AspNet");

        [Fact]
        public Task Providers_Neptune() => Verify("ExRam.Gremlinq.Providers.Neptune");

        [Fact]
        public Task Providers_Neptune_AspNet() => Verify("ExRam.Gremlinq.Providers.Neptune.AspNet");

        [Fact]
        public Task Support_NewtonsoftJson() => Verify("ExRam.Gremlinq.Support.NewtonsoftJson");

        [Fact]
        public Task Support_NewtonsoftJson_AspNet() => Verify("ExRam.Gremlinq.Support.NewtonsoftJson.AspNet");

        [Fact]
        public Task Support_TestContainers() => Verify("ExRam.Gremlinq.Support.TestContainers");

        private Task Verify(string assemblyName) => Verifier
            .Verify(
                Assembly
                    .Load(assemblyName)
                    .GeneratePublicApi(new ApiGeneratorOptions
                    {
                        IncludeAssemblyAttributes = false,
                        DenyNamespacePrefixes = []
                    }),
                "cs")
            .UniqueForTargetFrameworkAndVersion();
    }
}
