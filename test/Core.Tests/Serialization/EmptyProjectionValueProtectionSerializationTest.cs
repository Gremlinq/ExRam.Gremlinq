using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class EmptyProjectionValueProtectionSerializationTest : QueryExecutionTest, IClassFixture<EmptyProjectionValueProtectionSerializationTest.EmptyProjectionValueProtectionFixture>
    {
        public sealed class EmptyProjectionValueProtectionFixture : GremlinqFixture
        {
            protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)));
        }

        public EmptyProjectionValueProtectionSerializationTest(EmptyProjectionValueProtectionFixture fixture, ITestOutputHelper testOutputHelper) : base(
            fixture,
            new SerializingVerifier<Bytecode>(),
            testOutputHelper)
        {
        }
    }
}
