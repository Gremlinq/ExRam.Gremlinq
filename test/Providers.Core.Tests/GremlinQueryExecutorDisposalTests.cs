using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Execution;

using FluentAssertions;

using NSubstitute;

namespace ExRam.Gremlinq.Providers.Core.Tests
{
    public class GremlinQueryExecutorDisposalTests
    {
        [Fact]
        public async Task Executor_from_environment_disposes_factory_if_IAsyncDisposable()
        {
            var factory = Substitute.For<IGremlinqClientFactory, IAsyncDisposable>();

            var environment = GremlinQueryEnvironment.Invalid
                .UseExecutor(factory.ToExecutor());

            if (environment.Executor is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();

            await ((IAsyncDisposable)factory)
                .Received(1)
                .DisposeAsync();
        }

        [Fact]
        public async Task Executor_dispose_without_IAsyncDisposable_factory_does_not_throw()
        {
            var factory = Substitute.For<IGremlinqClientFactory>();

            var executor = factory.ToExecutor();

            executor
                .Should()
                .BeAssignableTo<IAsyncDisposable>();

            await ((IAsyncDisposable)executor).DisposeAsync();
        }

        [Fact]
        public async Task Pool_factory_disposes_base_factory_if_IAsyncDisposable()
        {
            var baseFactory = Substitute.For<IGremlinqClientFactory, IAsyncDisposable>();

            var executor = baseFactory
                .Pool()
                .ToExecutor();

            await ((IAsyncDisposable)executor).DisposeAsync();

            await ((IAsyncDisposable)baseFactory)
                .Received(1)
                .DisposeAsync();
        }

        [Fact]
        public async Task Pool_factory_dispose_without_IAsyncDisposable_base_does_not_throw()
        {
            var baseFactory = Substitute.For<IGremlinqClientFactory>();

            var executor = baseFactory
                .Pool()
                .ToExecutor();

            await ((IAsyncDisposable)executor).DisposeAsync();
        }

        [Fact]
        public async Task Executor_from_environment_disposes_wrapped_factory_executor()
        {
            var factory = Substitute.For<IGremlinqClientFactory, IAsyncDisposable>();

            var environment = GremlinQueryEnvironment.Invalid
                .UseExecutor(factory
                    .ToExecutor()
                    .TransformExecutionException(ex => ex));

            if (environment.Executor is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();

            await ((IAsyncDisposable)factory)
                .Received(1)
                .DisposeAsync();
        }

        [Fact(Skip = "Fails until GremlinQueryExecutorImpl disposes cached clients on disposal.")]
        public async Task Executor_dispose_disposes_cached_clients()
        {
            var factory = Substitute.For<IGremlinqClientFactory, IAsyncDisposable>();
            var client = Substitute.For<IGremlinqClient>();

            factory
                .Create(Arg.Any<IGremlinQueryEnvironment>())
                .Returns(client);

            var executor = factory.ToExecutor();

            try
            {
                await foreach (var _ in executor.Execute<object>(GremlinQueryExecutionContext.Create(GremlinQuerySource.g.V())))
                {
                }
            }
            catch
            {
            }

            factory
                .Received(1)
                .Create(Arg.Any<IGremlinQueryEnvironment>());

            await ((IAsyncDisposable)executor).DisposeAsync();

            client
                .Received(1)
                .Dispose();
        }
    }
}