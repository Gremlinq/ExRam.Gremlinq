using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Moq;
using Xunit;
using Unit = System.Reactive.Unit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryProviderTest
    {
        [Fact]
        public void WithSubgraphStrategyTest()
        {
            var query = GremlinQuery
                .Create()
                .WithSubgraphStrategy(_ => _.OfType<User>(), _ => _);

            query.Steps.Should().HaveCount(2);
            query.Steps[1].Should().BeOfType<MethodGremlinStep>();
            query.Steps[1].As<MethodGremlinStep>().Name.Should().Be("withStrategies");
            query.Steps[1].As<MethodGremlinStep>().Parameters.Should().HaveCount(1);
        }

        [Fact]
        public void WithSubgraphStrategy_is_ommitted_if_empty()
        {
            var query = GremlinQuery
                .Create()
                .WithSubgraphStrategy(_ => _, _ => _);

            query.Steps.Should().HaveCount(1);
            query.Steps[0].Should().BeOfType<IdentifierGremlinStep>();
        }

        [Fact]
        public void RewriteStepsTest()
        {
            var queryProviderMock = new Mock<IModelGremlinQueryProvider>();
            var subgraphStrategyProvider = queryProviderMock.Object.RewriteSteps<AddElementPropertiesStep>(
                step =>
                {
                    return step.Element is User
                        ? new[] { new ReplaceElementPropertyStep<User, int>(step, user => user.Age, 36) }
                        : Option<IEnumerable<GremlinStep>>.None;
                });

            subgraphStrategyProvider
                .Execute(GremlinQuery.Create().AddV(new User()));

            queryProviderMock.Verify(x => x.Execute(It.Is<IGremlinQuery<Unit>>(query => query.Steps[2] is ReplaceElementPropertyStep<User, int>)));
        }

        [Fact]
        public async Task Scalar()
        {
            var queryProviderMock = new Mock<INativeGremlinQueryProvider>();
            queryProviderMock
                .Setup(x => x.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()))
                .Returns(AsyncEnumerable.Return("[ 36 ]"));

            var value = await queryProviderMock.Object
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple))
                .WithJsonSupport()
                .Execute(GremlinQuery.Create().Cast<int>())
                .First();

            value.Should().Be(36);
        }
    }
}