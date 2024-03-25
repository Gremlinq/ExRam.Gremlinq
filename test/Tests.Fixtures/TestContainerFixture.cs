using System.Collections.Immutable;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class ImageTestContainerFixture : TestContainerFixtureBase
    {
        private readonly string _image;

        protected ImageTestContainerFixture(string image, int port = 8182) : base(port)
        {
            _image = image;
        }

        protected override async Task<IImage> GetImage() => new DockerImage(_image);
    }

    public abstract class DockerfileTestContainerFixture : TestContainerFixtureBase
    {
        private readonly string _dockerfile;

        protected DockerfileTestContainerFixture(string dockerfile, int port = 8182) : base(port)
        {
            _dockerfile = dockerfile;
        }

        protected override async Task<IImage> GetImage()
        {
            var futureImage = new ImageFromDockerfileBuilder()
                .WithDockerfile(_dockerfile)
                .Build();

            await futureImage.CreateAsync();

            return futureImage;
        }
    }

    public abstract class TestContainerFixtureBase : GremlinqFixture
    {
        private sealed class ContainerAttachedGremlinQuerySource : IGremlinQuerySource, IAsyncDisposable
        {
            private readonly IContainer _container;
            private readonly IGremlinQuerySource _baseSource;

            public ContainerAttachedGremlinQuerySource(IContainer container, IGremlinQuerySource baseSource)
            {
                _container = container;
                _baseSource = baseSource;
            }

            IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge) => _baseSource.AddE(edge);

            IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>() => _baseSource.AddE<TEdge>();

            IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex) => _baseSource.AddV(vertex);

            IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>() => _baseSource.AddV<TVertex>();

            IGremlinQueryAdmin IStartGremlinQuery.AsAdmin() => _baseSource.AsAdmin();

            IGremlinQuerySource IGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation) => _baseSource.ConfigureEnvironment(environmentTransformation);

            IEdgeGremlinQuery<object> IStartGremlinQuery.E(object id) => _baseSource.E(id);

            IEdgeGremlinQuery<object> IStartGremlinQuery.E(params object[] ids) => _baseSource.E(ids);

            IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(object id) => _baseSource.E<TEdge>(id);

            IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(params object[] ids) => _baseSource.E<TEdge>(ids);

            IGremlinQuery<TElement> IStartGremlinQuery.Inject<TElement>(params TElement[] elements) => _baseSource.Inject(elements);

            IEdgeGremlinQuery<TNewEdge> IStartGremlinQuery.ReplaceE<TNewEdge>(TNewEdge edge) => _baseSource.ReplaceE(edge);

            IVertexGremlinQuery<TNewVertex> IStartGremlinQuery.ReplaceV<TNewVertex>(TNewVertex vertex) => _baseSource.ReplaceV(vertex);

            IVertexGremlinQuery<object> IStartGremlinQuery.V(object id) => _baseSource.V(id);

            IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids) => _baseSource.V(ids);

            IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(object id) => _baseSource.V<TVertex>(id);

            IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params object[] ids) => _baseSource.V<TVertex>(ids);

            IGremlinQuerySource IGremlinQuerySource.WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => _baseSource.WithSideEffect(label, value);

            TQuery IGremlinQuerySource.WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation) => _baseSource.WithSideEffect(value, continuation);

            IGremlinQuerySource IGremlinQuerySource.ConfigureMetadata(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) => _baseSource.ConfigureMetadata(metadataTransformation);

            async ValueTask IAsyncDisposable.DisposeAsync()
            {
                await using(_container)
                {
                    await _container.StopAsync();
                }
            }
        }

        private readonly int _port;

        protected TestContainerFixtureBase(int port = 8182)
        {
            _port = port;
        }

        protected override sealed async Task<IGremlinQuerySource> TransformQuerySource(IGremlinQuerySource g)
        {
            var containerBuilder = new ContainerBuilder()
                .WithImage(await GetImage())
                .WithPortBinding(_port, true)
                .WithWaitStrategy(Wait
                    .ForUnixContainer()
                    .UntilPortIsAvailable(_port));

            var container = CustomizeContainer(containerBuilder)
                .Build();

            await container
                .StartAsync();

            return await TransformQuerySource(container, new ContainerAttachedGremlinQuerySource(container, g));
        }

        protected abstract Task<IImage> GetImage(); 

        protected virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder) => builder;

        protected abstract Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IGremlinQuerySource g);
    }
}
