using ExRam.Gremlinq.Core;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GroovyGremlinQuerySerializationFixture : GremlinqFixture
    {
        public GroovyGremlinQuerySerializationFixture() : base(g.ConfigureEnvironment(_ => _
            .ConfigureOptions(options => options
                .SetValue(GremlinqOption.PreferGroovySerialization, true))))
        {
        }
    }
}
