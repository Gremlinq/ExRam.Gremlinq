using System.Reflection;
using PublicApiGenerator;
using Xunit;
using VerifyXunit;
using System.Threading.Tasks;
using VerifyTests;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.PublicApi.Tests
{
    public class PublicApiTests
    {
        [Fact]
        public Task Core() => Verify("ExRam.Gremlinq.Core");

        [Fact]
        public Task CosmosDb() => Verify("ExRam.Gremlinq.Providers.CosmosDb");

        [Fact]
        public Task GremlinServer() => Verify("ExRam.Gremlinq.Providers.GremlinServer");

        [Fact]
        public Task Neptune() => Verify("ExRam.Gremlinq.Providers.Neptune");

        [Fact]
        public Task JanusGraph() => Verify("ExRam.Gremlinq.Providers.JanusGraph");

        [Fact]
        public Task WebSocket() => Verify("ExRam.Gremlinq.Providers.WebSocket");

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
        public Task WebSocketAspNet() => Verify("ExRam.Gremlinq.Providers.WebSocket.AspNet");

        private static Task Verify(string assemblyName, [CallerFilePath] string sourceFile = "")
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseExtension("cs");

            var options = new ApiGeneratorOptions
            {
                IncludeAssemblyAttributes = false
            };

            return Verifier.Verify(
                Assembly
                    .Load(assemblyName)
                    .GeneratePublicApi(options),
                verifySettings,
                sourceFile);
        }
    }
}
