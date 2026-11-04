using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class EmptyProjectionValueProtectionSerializationTests : QueryExecutionTestsBase, IClassFixture<EmptyProjectionValueProtectionSerializationTests.Fixture>
    {
        public sealed class Fixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)));
        }

        public EmptyProjectionValueProtectionSerializationTests(Fixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {
        }
    }
}
