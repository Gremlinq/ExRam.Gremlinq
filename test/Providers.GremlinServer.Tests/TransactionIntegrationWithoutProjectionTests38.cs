using DotNet.Testcontainers.Builders;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> without element projection against Gremlin Server 3.8.2 with
    /// <c>TinkerTransactionGraph</c>. Owns the snapshots of the facts on a failed write, which only
    /// <see cref="TransactionIntegrationWithoutProjectionTests38Base"/> holds; every other fact verifies against the snapshots of
    /// <see cref="IntegrationWithoutProjectionTests38Base"/>.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class TransactionIntegrationWithoutProjectionTests38 : TransactionIntegrationWithoutProjectionTests38Base, IClassFixture<TransactionIntegrationWithoutProjectionTests38.Fixture>, ISourceFileNameProvider<TransactionIntegrationWithoutProjectionTests38>
    {
        public sealed class Fixture : GremlinServerFixture
        {
            protected override IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator) => configurator
                .UseTestContainers(c => c
                    .ConfigureContainer(builder => builder
                        .WithImage($"tinkerpop/gremlin-server:{TinkerPopVersions.V3_8_2}")
                        .WithCommand("conf/gremlin-server-transaction.yaml")
                        .WithPortBinding(8182, true)
                        .WithWaitStrategy(Wait
                            .ForUnixContainer()
                            .UntilInternalTcpPortIsAvailable(8182)))
                    .ConfigureClientFactory((poolFactory, container) => poolFactory
                        .ConfigureBaseFactory(webSocketFactory => webSocketFactory
                            .ConfigureUri(_ => new Uri($"ws://localhost:{container.GetMappedPublicPort(8182)}")))))
                .ConfigureServerCompatibilityRange(_ => VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2));

            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .IgnoreCosmosDbSpecificProperties()
                .WithoutElementProjection();
        }

        public TransactionIntegrationWithoutProjectionTests38(Fixture fixture) : base(fixture, new ExecutingVerifier())
        {
        }

        public static string GetSourceFileName() => SourceFileName.OfThis();
    }
}
