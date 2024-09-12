using Docker.DotNet;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

using Microsoft.Extensions.Logging;

using Xunit.Sdk;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class TestContainerFixtureBase : GremlinqFixture
    {
        private sealed class DiagnosticMessageLogger : ILogger
        {
            private readonly IMessageSink _sink;

            public DiagnosticMessageLogger(IMessageSink sink)
            {
                _sink = sink;
            }

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                _sink.OnMessage(new DiagnosticMessage(formatter(state, exception)));
            }
        }

        private readonly int _port;
        private readonly IMessageSink _messageSink;

        private IContainer? _container;

        protected TestContainerFixtureBase(int port, IMessageSink messageSink)
        {
            _port = port;
            _messageSink = messageSink;
        }

        protected sealed override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g)
        {
            if (_container is { } container)
                return TransformQuerySource(container, g);

            throw new InvalidOperationException();
        }

        public override async Task InitializeAsync()
        {
            var containerBuilder = new ContainerBuilder()
                .WithImage(await GetImage())
                .WithName(Guid.NewGuid().ToString("N"))
                .WithPortBinding(_port, true)
                .WithAutoRemove(true)
                .WithWaitStrategy(Wait
                    .ForUnixContainer()
                    .UntilPortIsAvailable(_port))
                .WithReuse(false)
                .WithLogger(new DiagnosticMessageLogger(_messageSink));

            _container = CustomizeContainer(containerBuilder)
                .Build();

            await _container
                .StartAsync();

            await base.InitializeAsync();
        }

        public override async Task DisposeAsync()
        {
            if (_container is { } container)
            {
                try
                {
                    await using (container)
                    {
                        await container.StopAsync();
                    }
                }
                catch (DockerContainerNotFoundException)
                {

                }
            }

            await base.DisposeAsync();
        }

        protected abstract Task<IImage> GetImage(); 

        protected virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder) => builder;

        protected abstract IGremlinQuerySource TransformQuerySource(IContainer container, IGremlinQuerySource g);
    }
}
