using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The customized Gremlin Server of the given version, 3.7.7 by default, with every query sent as a Groovy script. A
    /// fixture for another version calls the protected constructor.
    /// </summary>
    public class GroovyGremlinServerContainerFixture : CustomGremlinServerContainerFixture
    {
        public GroovyGremlinServerContainerFixture() : base()
        {
        }

        protected GroovyGremlinServerContainerFixture(Version version) : base(version)
        {
        }

        protected override IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g) => base
            .TransformQuerySource(container, g)
            .SerializeToGroovy();
    }
}
