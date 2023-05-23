using ExRam.Gremlinq.Core.Serialization;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public sealed class GroovyGremlinQuerySerializationFixture : GremlinqFixture
    {
        public GroovyGremlinQuerySerializationFixture() : base(g.ConfigureEnvironment(_ => _
            .ConfigureSerializer(ser => ser
                .PreferGroovySerialization())))
        {
        }
    }
}
