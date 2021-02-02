using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbDeserializationTests : QueryDeserializationTest
    {
        public CosmosDbDeserializationTests(ITestOutputHelper testOutputHelper) : base(
            GremlinQuerySource.g.ConfigureEnvironment(
                env => env.UseCosmosDb(_ => _.At("ws://localhost", "", "").AuthenticateBy(""))),
            testOutputHelper)
        {
        }

        [Fact(Skip="Wrong Choose signature in Gremlinq <= 8!")]
        public override Task Choose_Predicate2()
        {
            return base.Choose_Predicate2();
        }
    }
}
