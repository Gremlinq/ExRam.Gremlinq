using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class ObjectDeserializationTests : DeserializationTestsBase, IClassFixture<ObjectDeserializationTests.ObjectDeserializationFixture>
    {
        public sealed class ObjectDeserializationFixture : DeserializationTestFixture
        {
            public ObjectDeserializationFixture() : base(g
                .UseGremlinServer(_ => _
                    .UseNewtonsoftJson()))
            {
            }

            public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<object>());
        }

        public ObjectDeserializationTests(ObjectDeserializationFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }
    }
}
