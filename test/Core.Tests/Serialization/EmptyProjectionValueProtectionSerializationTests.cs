using ExRam.Gremlinq.Tests.Infrastructure;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Tests
{
    public sealed class EmptyProjectionValueProtectionSerializationTests : QueryExecutionTestsBase, IClassFixture<EmptyProjectionValueProtectionSerializationTests.EmptyProjectionValueProtectionFixture>
    {
        public class EmptyProjectionValueProtectionFixture : GremlinqFixture
        {
            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)));
        }

        public EmptyProjectionValueProtectionSerializationTests(EmptyProjectionValueProtectionFixture fixture) : base(
            fixture,
            new SerializingVerifier<Bytecode>())
        {
        }
    }
}
