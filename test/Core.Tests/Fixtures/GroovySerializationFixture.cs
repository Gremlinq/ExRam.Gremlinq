using ExRam.Gremlinq.Core.Serialization;
using static ExRam.Gremlinq.Core.GremlinQuerySource;
using static ExRam.Gremlinq.Core.Tests.SerializationTestsBase;

namespace ExRam.Gremlinq.Core.Tests.Fixtures
{
    public sealed class GroovySerializationFixture : SerializationTestsFixture<GroovyGremlinQuery>
    {
        public GroovySerializationFixture() : base(g.ConfigureEnvironment(_ => _
            .ConfigureSerializer(ser => ser
                .PreferGroovySerialization())))
        {
        }
    }
}
