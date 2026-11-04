using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;

using VerifyTests.MicrosoftLogging;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// Gremlin Server 3.7.7 with a logger that records what the provider logs, for <see cref="LoggingVerifier"/>. The logger's
    /// category is this class's name, which the snapshots show.
    /// </summary>
    public class LoggingGremlinServerContainerFixture : GremlinServerContainerFixture
    {
        public override IGremlinQuerySource GetQuerySource() => base
            .GetQuerySource()
            .ConfigureEnvironment(env => env
                .ConfigureLogger(_ => RecordingProvider.CreateLogger<LoggingGremlinServerContainerFixture>()));
    }
}
