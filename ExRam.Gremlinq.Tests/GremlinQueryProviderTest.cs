using System.Reactive;
using Moq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryProviderTest
    {
        [Fact]
        public void WithSubgraphStrategyTest()
        {
            var queryProviderMock = new Mock<IGremlinQueryProvider>();
            var subgraphStrategyProvider = queryProviderMock.Object.WithSubgraphStrategy(_ => _, _ => _);

            subgraphStrategyProvider
                .Execute(GremlinQuery.Create("g").Cast<Unit>());

            queryProviderMock.Verify(x => x.Execute<Unit>(It.Is<IGremlinQuery<Unit>>(query => (query.Steps[0] is TerminalGremlinStep) && ((TerminalGremlinStep)query.Steps[0]).Name == "withStrategies" && ((TerminalGremlinStep)query.Steps[0]).Parameters.Count == 1)));
        }
    }
}