using System.Reflection;
using PublicApiGenerator;

namespace ExRam.Gremlinq.PublicApi.Tests
{
    public class PublicApiTests : VerifyBase
    {
        public PublicApiTests() : base()
        {

        }

        [Fact]
        public Task Core() => Verify("ExRam.Gremlinq.Core");

        [Fact]
        public Task ProvidersCore() => Verify("ExRam.Gremlinq.Providers.Core");

        [Fact]
        public Task CosmosDb() => Verify("ExRam.Gremlinq.Providers.CosmosDb");

        [Fact]
        public Task GremlinServer() => Verify("ExRam.Gremlinq.Providers.GremlinServer");

        [Fact]
        public Task Neptune() => Verify("ExRam.Gremlinq.Providers.Neptune");

        [Fact]
        public Task JanusGraph() => Verify("ExRam.Gremlinq.Providers.JanusGraph");

        [Fact]
        public Task CoreAspNet() => Verify("ExRam.Gremlinq.Core.AspNet");

        [Fact]
        public Task CosmosDbAspNet() => Verify("ExRam.Gremlinq.Providers.CosmosDb.AspNet");

        [Fact]
        public Task GremlinServerAspNet() => Verify("ExRam.Gremlinq.Providers.GremlinServer.AspNet");

        [Fact]
        public Task NeptuneAspNet() => Verify("ExRam.Gremlinq.Providers.Neptune.AspNet");

        [Fact]
        public Task JanusGraphAspNet() => Verify("ExRam.Gremlinq.Providers.JanusGraph.AspNet");

        [Fact]
        public Task SupportNewtonsoftJson() => Verify("ExRam.Gremlinq.Support.NewtonsoftJson");

        private Task Verify(string assemblyName) => base
            .Verify(
                Assembly
                    .Load(assemblyName)
                    .GeneratePublicApi(new ApiGeneratorOptions
                    {
                        IncludeAssemblyAttributes = false,
                        DenyNamespacePrefixes = Array.Empty<string>()
                    }),
                "cs")
            .UniqueForTargetFrameworkAndVersion();
    }
}
