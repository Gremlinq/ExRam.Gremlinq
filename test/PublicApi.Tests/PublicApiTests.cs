using System.Reflection;
using System.Runtime.CompilerServices;

using PublicApiGenerator;

namespace ExRam.Gremlinq.PublicApi.Tests
{
    public class PublicApiTests
    {
        [Fact]
        public Task Core() => Verify();

        [Fact]
        public Task Core_AspNet() => Verify();

        [Fact]
        public Task Providers_Core() => Verify();

        [Fact]
        public Task Providers_CosmosDb() => Verify();

        [Fact]
        public Task Providers_CosmosDb_AspNet() => Verify();

        [Fact]
        public Task Providers_GremlinServer() => Verify();

        [Fact]
        public Task Providers_GremlinServer_AspNet() => Verify();

        [Fact]
        public Task Providers_JanusGraph() => Verify();

        [Fact]
        public Task Providers_JanusGraph_AspNet() => Verify();

        [Fact]
        public Task Providers_Neptune() => Verify();

        [Fact]
        public Task Providers_Neptune_AspNet() => Verify();

        [Fact]
        public Task Support_NewtonsoftJson() => Verify();

        [Fact]
        public Task Support_NewtonsoftJson_AspNet() => Verify();

        [Fact]
        public Task Support_TestContainers() => Verify();

        [Fact]
        public Task Testing_AirRoutes() => Verify();

        private static Task Verify([CallerMemberName] string methodName = "") => Verifier
            .Verify(
                Assembly
                    .Load("ExRam.Gremlinq." + methodName.Replace('_', '.'))
                    .GeneratePublicApi(new ApiGeneratorOptions
                    {
                        IncludeAssemblyAttributes = false,
                        DenyNamespacePrefixes = []
                    }),
                "cs")
            .UniqueForTargetFrameworkAndVersion();
    }
}
