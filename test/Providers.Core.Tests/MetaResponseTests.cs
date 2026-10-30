using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using Gremlin.Net.Driver.Messages;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class MetaResponseTests
    {
        [Fact]
        public async Task Response_without_data_gives_a_MetaResponse_without_data()
        {
            var client = Substitute.For<IGremlinqClient>();
            var factory = Substitute.For<IGremlinqClientFactory>();

            factory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(client);

            client
                .SubmitAsync<List<object>>(Arg.Any<RequestMessage>())
                .Returns(callInfo => new[] { new ResponseMessage<List<object>>(callInfo.Arg<RequestMessage>().RequestId, new ResponseStatus(ResponseStatusCode.NoContent), new ResponseResult<List<object>>(null)) }
                    .ToAsyncEnumerable());

            var context = GremlinQueryExecutionContext.Create(GremlinQuerySource.g.V());
            var responses = new List<MetaResponse<object>>();

            await foreach (var response in factory.ToExecutor().Execute<MetaResponse<object>>(context).WithCancellation(TestContext.Current.CancellationToken))
            {
                responses.Add(response);
            }

            var metaResponse = responses
                .Should()
                .ContainSingle()
                .Which;

            metaResponse.Data.Should().BeNull();
            metaResponse.RequestId.Should().Be(context.ExecutionId);
            metaResponse.ResponseStatus.Code.Should().Be(ResponseStatusCode.NoContent);
        }

        [Fact]
        public async Task Response_with_data_gives_a_MetaResponse_with_that_data()
        {
            var client = Substitute.For<IGremlinqClient>();
            var factory = Substitute.For<IGremlinqClientFactory>();

            factory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(client);

            client
                .SubmitAsync<List<object>>(Arg.Any<RequestMessage>())
                .Returns(callInfo => new[] { new ResponseMessage<List<object>>(callInfo.Arg<RequestMessage>().RequestId, new ResponseStatus(ResponseStatusCode.Success), new ResponseResult<List<object>>([1, 2])) }
                    .ToAsyncEnumerable());

            var responses = new List<MetaResponse<object>>();

            await foreach (var response in factory.ToExecutor().Execute<MetaResponse<object>>(GremlinQueryExecutionContext.Create(GremlinQuerySource.g.V())).WithCancellation(TestContext.Current.CancellationToken))
            {
                responses.Add(response);
            }

            var metaResponse = responses
                .Should()
                .ContainSingle()
                .Which;

            metaResponse.Data.Should().Equal(1, 2);
            metaResponse.ResponseStatus.Code.Should().Be(ResponseStatusCode.Success);
        }
    }
}
