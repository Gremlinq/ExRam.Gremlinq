using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class OutOfTheBoxTest : GremlinqTestBase
    {
        private class SomeEntity
        {

        }

        public OutOfTheBoxTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public async Task Execution()
        {
            g
                .Awaiting(async _ => await _
                    .ConfigureEnvironment(_ => _)
                    .V()
                    .ToArrayAsync())
                .Should()
                .Throw<InvalidOperationException>()
                .Where(x => x.Message.StartsWith("'Execute' must not be called on GremlinQueryExecutor.Invalid"));
        }

        [Fact]
        public async Task V_SomeEntity()
        {
            await g
                .ConfigureEnvironment(e => e
                    .ConfigureSerializer(s => s.ToGroovy())
                    .UseExecutor(GremlinQueryExecutor.Identity)
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default))
                .V<SomeEntity>()
                .Cast<GroovyGremlinQuery>()
                .Verify();
        }
    }
}
