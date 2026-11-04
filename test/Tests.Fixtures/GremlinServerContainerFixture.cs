using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.GremlinServer;
using ExRam.Gremlinq.Support.TestContainers;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// A Gremlin Server of the given version in a container. A fixture for another version or another compatibility range
    /// calls the protected constructor; one for another JSON stack overrides <see cref="GremlinServerFixture.AddJsonSupport"/>;
    /// one that changes the environment overrides <see cref="TransformQuerySource"/>.
    /// </summary>
    public class GremlinServerContainerFixture : GremlinServerFixture
    {
        private readonly Version _version;
        private readonly VersionRange? _range;

        public GremlinServerContainerFixture() : this(TinkerPopVersions.V3_7_7, null)
        {
        }

        protected GremlinServerContainerFixture(Version version, VersionRange? range)
        {
            _version = version;
            _range = range;
        }

        protected override IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator)
        {
            var containerConfigurator = configurator
                .UseGremlinServerContainer(_version.ToString());

            return _range is { } range
                ? containerConfigurator.ConfigureServerCompatibilityRange(_ => range)
                : containerConfigurator;
        }

        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
            .TransformQuerySource(g)
            .IgnoreCosmosDbSpecificProperties();
    }
}
