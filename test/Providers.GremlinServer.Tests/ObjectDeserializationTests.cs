using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class ObjectDeserializationTests : QueryExecutionTest, IClassFixture<ObjectDeserializationTests.ObjectDeserializationFixture>
    {
        public new sealed class Verifier : DeserializingGremlinqVerifier
        {
            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<object>());
        }

        public sealed class ObjectDeserializationFixture : GremlinqTestFixture
        {
            public ObjectDeserializationFixture() : base(g
                .UseGremlinServer(_ => _
                    .UseNewtonsoftJson()))
            {
            }

        }

        public ObjectDeserializationTests(ObjectDeserializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new Verifier(),
            testOutputHelper)
        {
        }
    }
}
