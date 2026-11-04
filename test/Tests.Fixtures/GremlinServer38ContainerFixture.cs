using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// A Gremlin Server 3.8.2 in a container, for a compatibility range of 3.8.2 only.
    /// </summary>
    public class GremlinServer38ContainerFixture : GremlinServerContainerFixture
    {
        public GremlinServer38ContainerFixture() : base(TinkerPopVersions.V3_8_2, VersionRange.From(TinkerPopVersions.V3_8_2, TinkerPopVersions.V3_8_2))
        {
        }
    }
}
