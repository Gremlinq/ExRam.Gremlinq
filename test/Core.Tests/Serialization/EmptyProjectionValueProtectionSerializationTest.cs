using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;
using ExRam.Gremlinq.Tests.TestCases;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public class EmptyProjectionValueProtectionSerializationTest : QueryExecutionTest, IClassFixture<EmptyProjectionValueProtectionSerializationTest.EmptyProjectionValueProtectionFixture>
    {
        public class EmptyProjectionValueProtectionFixture : GremlinqFixture
        {
            protected override async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)));
        }

        public EmptyProjectionValueProtectionSerializationTest(EmptyProjectionValueProtectionFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {
        }
    }
}
