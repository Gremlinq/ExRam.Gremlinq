using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Neptune;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// The Neptune provider with IAM authentication against a Gremlin Server of the given version in a container, for the
    /// given Neptune engine, read through Newtonsoft.Json.
    /// </summary>
    public class NeptuneContainerFixture : GremlinqFixture
    {
        private readonly Version _serverVersion;
        private readonly Version? _engineVersion;

        public NeptuneContainerFixture() : this(TinkerPopVersions.V3_7_7, null)
        {
        }

        protected NeptuneContainerFixture(Version serverVersion, Version? engineVersion)
        {
            _serverVersion = serverVersion;
            _engineVersion = engineVersion;
        }

        protected virtual INeptuneConfigurator ConfigureProvider(INeptuneConfigurator configurator)
        {
            var containerConfigurator = configurator
                .UseIAMAuthentication(_ => _
                    .UseSigV4()
                    .WithUri(new Uri("http://localhost:8182"))
                    .WithAccessKeyId("accessKeyId")
                    .WithSecretAccessKey("secretAccessKey"))
                .UseGremlinServerContainer(_serverVersion.ToString());

            return _engineVersion is { } engineVersion
                ? containerConfigurator.WithEngineVersion(engineVersion)
                : containerConfigurator;
        }

        protected virtual INeptuneConfigurator AddJsonSupport(INeptuneConfigurator configurator) => configurator
            .UseNewtonsoftJson();

        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .UseNeptune<Vertex, Edge>(configurator => AddJsonSupport(ConfigureProvider(configurator)))
            .IgnoreCosmosDbSpecificProperties();
    }
}
