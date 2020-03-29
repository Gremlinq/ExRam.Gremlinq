using System.Reflection;
using System.Runtime.CompilerServices;
//using ApprovalTests.Reporters;
//using ApprovalTests.Reporters.Windows;
using PublicApiGenerator;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.ApprovalTests
{
    public class ApprovalTests : VerifyXunit.VerifyBase
    {
        public ApprovalTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void Core()
        {
            Assembly
                .Load("ExRam.Gremlinq.Core")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void CosmosDb()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void GremlinServer()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void Neptune()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void WebSocket()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void CoreAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Core.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void CosmosDbAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.CosmosDb.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void GremlinServerAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.GremlinServer.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void NeptuneAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.Neptune.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }

        [Fact]
        public void WebSocketAspNet()
        {
            Assembly
                .Load("ExRam.Gremlinq.Providers.WebSocket.AspNet")
                .GeneratePublicApi()
                .VerifyCSharp(this);
        }
    }
}
