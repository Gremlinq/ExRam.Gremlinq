using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson.Tests;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public sealed class DictionaryDeserializationTests : DeserializationTestsBase, IClassFixture<DictionaryDeserializationTests.Fixture>
    {
        public new sealed class Fixture : DeserializationTestsBase.Fixture
        {
            public Fixture() : base(
                nameof(IntegrationTests),
                g
                    .UseGremlinServer(_ => _
                        .UseNewtonsoftJson()))
            {
            }
        }

        public DictionaryDeserializationTests(Fixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            testOutputHelper)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query) => base.Verify(query.Cast<object>());
    }
}
