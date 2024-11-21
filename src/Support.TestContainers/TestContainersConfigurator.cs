using System.Runtime.CompilerServices;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Support.TestContainers
{
    public readonly struct TestContainersConfigurator
    {
        private sealed class ContainerGremlinqClientFactory : IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>
        {
            private sealed class ContainerGremlinClient : IGremlinqClient
            {
                private object? _baseClient;

                private readonly IGremlinQueryEnvironment _environment;
                private readonly ContainerGremlinqClientFactory _parentFactory;

                public ContainerGremlinClient(ContainerGremlinqClientFactory parentFactory, IGremlinQueryEnvironment environment)
                {
                    _environment = environment;
                    _parentFactory = parentFactory;
                }

                public void Dispose() => (Interlocked.Exchange(ref _baseClient, DisposedObject) as IDisposable)?.Dispose();

                public IAsyncEnumerable<ResponseMessage<T>> SubmitAsync<T>(RequestMessage message)
                {
                    return Core(this, message);

                    static async IAsyncEnumerable<ResponseMessage<T>> Core(ContainerGremlinClient @this, RequestMessage message, [EnumeratorCancellation] CancellationToken ct = default)
                    {
                        if (await @this.TryGetBaseClient(ct) is { } baseClient)
                        {
                            await foreach (var item in baseClient.SubmitAsync<T>(message).WithCancellation(ct).ConfigureAwait(false))
                            {
                                yield return item;
                            }
                        }
                    }
                }

                private async ValueTask<IGremlinqClient?> TryGetBaseClient(CancellationToken ct)
                {
                    while (true)
                    {
                        if (Volatile.Read(ref _baseClient) is { } baseClientObject)
                        {
                            if (ReferenceEquals(baseClientObject, DisposedObject))
                                return null;

                            if (!ReferenceEquals(baseClientObject, InProgressObject))
                                return baseClientObject as IGremlinqClient;
                        }

                        if (Interlocked.CompareExchange(ref _baseClient, InProgressObject, null) == null)
                        {
                            if (await _parentFactory.TryCreateContainer(ct) is { } container)
                            {
                                var newClient = _parentFactory._baseFactory
                                    .ConfigureBaseFactory(baseFactory => baseFactory
                                        .ConfigureUri(_ => new Uri($"ws://localhost:{container.GetMappedPublicPort(8182)}")))
                                    .Create(_environment);

                                if (Interlocked.CompareExchange(ref _baseClient, newClient, InProgressObject) == InProgressObject)
                                    return newClient;

                                newClient.Dispose();
                            }
                        }
                    }
                }
            }

            private static readonly object DisposedObject = new ();
            private static readonly object InProgressObject = new();

            private object? _container;

            private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> _baseFactory;
            private readonly Func<ContainerBuilder, ContainerBuilder> _containerBuilderTransformation;

            public ContainerGremlinqClientFactory(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> baseFactory, Func<ContainerBuilder, ContainerBuilder> containerBuilderTransformation)
            {
                _baseFactory = baseFactory;
                _containerBuilderTransformation = containerBuilderTransformation;
            }

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new ContainerGremlinClient(this, environment);

            public IPoolGremlinqClientFactory<TNewBaseFactory> ConfigureBaseFactory<TNewBaseFactory>(Func<IWebSocketGremlinqClientFactory, TNewBaseFactory> transformation) where TNewBaseFactory : IGremlinqClientFactory => throw new NotSupportedException();

            public IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> ConfigureClient(Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation) => new ContainerGremlinqClientFactory(
                _baseFactory.ConfigureClient(clientTransformation),
                _containerBuilderTransformation);

            public IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> WithMaxInProcessPerConnection(int maxInProcessPerConnection) => throw new NotSupportedException();

            public IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> WithPoolSize(int poolSize) => throw new NotSupportedException();


            private async ValueTask<IContainer?> TryCreateContainer(CancellationToken ct)
            {
                while (true)
                {
                    if (Volatile.Read(ref _container) is { } containerObject)
                    {
                        if (ReferenceEquals(containerObject, DisposedObject))
                            return null;

                        if (!ReferenceEquals(containerObject, InProgressObject))
                            return containerObject as IContainer;
                    }

                    if (Interlocked.CompareExchange(ref _container, InProgressObject, null) == null)
                    {
                        var newContainer = _containerBuilderTransformation(new ContainerBuilder())
                            .Build();

                        if (Interlocked.CompareExchange(ref _container, newContainer, InProgressObject) == InProgressObject)
                        {
                            await newContainer.StartAsync(ct);

                            return newContainer;
                        }
                    }
                }
            }
        }

        private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>? _clientFactory;

        internal TestContainersConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> ConfigureContainer(Func<ContainerBuilder, ContainerBuilder> containerBuilderTransformation) => _clientFactory is { } clientFactory
            ? new ContainerGremlinqClientFactory(clientFactory, containerBuilderTransformation)
            : throw new InvalidOperationException();
    }
}
