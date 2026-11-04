using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    /// <summary>
    /// No provider; a query is serialized to a Groovy script.
    /// </summary>
    public class GroovyGremlinQuerySerializationFixture : GremlinqFixture
    {
        protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
            .SerializeToGroovy()
            .IgnoreCosmosDbSpecificProperties();
    }
}
