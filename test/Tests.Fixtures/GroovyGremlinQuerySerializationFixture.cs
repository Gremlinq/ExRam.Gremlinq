using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public sealed class GroovyGremlinQuerySerializationFixture : GremlinqFixture
    {
        protected override async Task<IGremlinQuerySource> TransformQuerySource(IConfigurableGremlinQuerySource g) => g.ConfigureEnvironment(_ => _
            .ConfigureOptions(options => options
                .SetValue(GremlinqOption.PreferGroovySerialization, true)));
    }
}
