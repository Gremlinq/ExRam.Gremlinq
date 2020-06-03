using System.Reflection;
using PublicApiGenerator;
using Xunit;
using Xunit.Abstractions;
using VerifyXunit;
using System.Threading.Tasks;

namespace ExRam.Gremlinq.ApprovalTests
{
    public class PubliApiTests : VerifyBase
    {
        public PubliApiTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public Task Core()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Core")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task CosmosDb()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task GremlinServer()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task Neptune()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task JanusGraph()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.JanusGraph")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task WebSocket()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task CoreAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Core.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task CosmosDbAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task GremlinServerAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task NeptuneAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task JanusGraphAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.JanusGraph.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public Task WebSocketAspNet()
        {
            return Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }
    }
}
