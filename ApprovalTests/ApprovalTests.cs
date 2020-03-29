using System.Reflection;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using Xunit;

namespace ExRam.Gremlinq.ApprovalTests
{
    [UseReporter(typeof(DiffReporter))]
    public class ApprovalTests
    {
        [Fact]
        public void Core()
        {
            Assembly
                .Load("ExRam.Gremlinq.Core")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void CosmosDb()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void GremlinServer()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void Neptune()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void WebSocket()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void CoreAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Core.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void CosmosDbAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void GremlinServerAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void NeptuneAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }

        [Fact]
        public void WebSocketAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp();
        }
    }
}
