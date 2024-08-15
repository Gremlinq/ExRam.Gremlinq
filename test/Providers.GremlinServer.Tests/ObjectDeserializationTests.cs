using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class ObjectDeserializationTests : QueryExecutionTest, IClassFixture<GremlinServerContainerFixture>
    {
        public ObjectDeserializationTests(GremlinServerContainerFixture fixture) : base(
            fixture,
            new ObjectQueryExecutingGremlinqVerifier())
        {
        }
    }
}
