using ExRam.Gremlinq.Core.Serialization;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests.Fixtures
{
    public sealed class GroovyGremlinQuerySerializationFixture : SerializationFixture<GroovyGremlinQuery>
    {
        public GroovyGremlinQuerySerializationFixture() : base(g.ConfigureEnvironment(_ => _
            .ConfigureSerializer(ser => ser
                .PreferGroovySerialization())))
        {
        }
    }
}
