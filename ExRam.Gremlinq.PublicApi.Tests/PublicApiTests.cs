using System.Reflection;
using PublicApiGenerator;
using Xunit;
using VerifyXunit;
using System.Threading.Tasks;

namespace ExRam.Gremlinq.PublicApi.Tests
{
    [UsesVerify]
    public class PublicApiTests
    {
        [Fact]
        public Task Core()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Core")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task CosmosDb()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task GremlinServer()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task Neptune()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task JanusGraph()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.JanusGraph")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task WebSocket()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task CoreAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Core.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task CosmosDbAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task GremlinServerAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task NeptuneAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task JanusGraphAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.JanusGraph.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public Task WebSocketAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }
    }
}
