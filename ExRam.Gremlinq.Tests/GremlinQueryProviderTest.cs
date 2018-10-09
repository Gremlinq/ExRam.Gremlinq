using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryProviderTest
    {
        [Fact]
        public void WithSubgraphStrategyTest()
        {
            var query = g
                .WithSubgraphStrategy(_ => _.OfType<User>(), _ => _);

            query.Steps.Should().HaveCount(2);
            query.Steps[1].Should().BeOfType<MethodGremlinStep>();
            query.Steps[1].As<MethodGremlinStep>().Name.Should().Be("withStrategies");
            query.Steps[1].As<MethodGremlinStep>().Parameters.Should().HaveCount(1);
        }

        [Fact]
        public void WithSubgraphStrategy_is_ommitted_if_empty()
        {
            var query = g
                .WithSubgraphStrategy(_ => _, _ => _);

            query.Steps.Should().HaveCount(1);
            query.Steps[0].Should().BeOfType<IdentifierGremlinStep>();
        }

        [Fact]
        public async Task Scalar()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider<string>>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ 36 ]"));

            var value = await queryProviderMock.Object
                .Select(JToken.Parse)
                .WithJsonSupport()
                .Execute(GremlinQuery
                    .Create("g")
                    .SetModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                    .Cast<int>())
                .First();

            value.Should().Be(36);
        }
    }
}