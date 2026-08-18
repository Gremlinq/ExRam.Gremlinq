using System.Runtime.CompilerServices;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Support.TestContainers
{
    /// <summary>Configurator for creating a client factory that is backed by a Testcontainers container.</summary>
    public readonly struct TestContainersWithContainerConfigurator
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
                        if (await @this.TryGetBaseClient(ct).ConfigureAwait(false) is { } baseClient)
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
                            if (await _parentFactory.TryCreateContainer(ct).ConfigureAwait(false) is { } container)
                            {
                                var newClient = _parentFactory._factoryTransformation(_parentFactory._baseFactory, container)
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
            private readonly Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IContainer, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> _factoryTransformation;

            public ContainerGremlinqClientFactory(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> baseFactory, Func<ContainerBuilder, ContainerBuilder> containerBuilderTransformation, Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IContainer, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> factoryTransformation)
            {
                _baseFactory = baseFactory;
                _factoryTransformation = factoryTransformation;
                _containerBuilderTransformation = containerBuilderTransformation;
            }

            public IGremlinqClient Create(IGremlinQueryEnvironment environment) => new ContainerGremlinClient(this, environment);

            public IPoolGremlinqClientFactory<TNewBaseFactory> ConfigureBaseFactory<TNewBaseFactory>(Func<IWebSocketGremlinqClientFactory, TNewBaseFactory> transformation) where TNewBaseFactory : IGremlinqClientFactory => throw new NotSupportedException();

            public IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> ConfigureClient(Func<IGremlinqClient, IGremlinQueryEnvironment, IGremlinqClient> clientTransformation) => new ContainerGremlinqClientFactory(
                _baseFactory.ConfigureClient(clientTransformation),
                _containerBuilderTransformation,
                _factoryTransformation);

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
                        var newContainer = _containerBuilderTransformation(new ContainerBuilder("tinkerpop/gremlin-server"))
                            .Build();

                        if (Interlocked.CompareExchange(ref _container, newContainer, InProgressObject) == InProgressObject)
                        {
                            await newContainer
                                .StartAsync(ct)
                                .ConfigureAwait(false);

                            return newContainer;
                        }

                        await newContainer
                            .DisposeAsync()
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        private readonly Func<ContainerBuilder, ContainerBuilder>? _containerBuilderTransformation;
        private readonly IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>? _clientFactory;

        internal TestContainersWithContainerConfigurator(IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> clientFactory, Func<ContainerBuilder, ContainerBuilder> containerBuilderTransformation)
        {
            _clientFactory = clientFactory;
            _containerBuilderTransformation = containerBuilderTransformation;
        }

        /// <summary>Configures the client factory using the running container.</summary>
        /// <param name="factoryTransformation">The factory transformation that receives the container.</param>
        public IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory> ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IContainer, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> factoryTransformation)
        {
            ArgumentNullException.ThrowIfNull(factoryTransformation);

            return _clientFactory is { } clientFactory && _containerBuilderTransformation is { } containerBuilderTransformation
                ? new ContainerGremlinqClientFactory(clientFactory, containerBuilderTransformation, factoryTransformation)
                : throw new InvalidOperationException();
        }
    }
}
