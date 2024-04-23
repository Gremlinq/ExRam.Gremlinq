using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;
using ExRam.Gremlinq.Core.Execution;
using Gremlin.Net.Driver.Exceptions;
using DotNet.Testcontainers.Containers;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Providers.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using static ExRam.Gremlinq.Providers.GremlinServer.Tests.WrongPasswordIntegrationTests;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    [IntegrationTest]
    public sealed class WrongPasswordIntegrationTests : GremlinqTestBase, IClassFixture<WrongPasswordGremlinServerContainerFixture>
    {
        public sealed class WrongPasswordGremlinServerContainerFixture : DockerfileTestContainerFixture
        {
            public WrongPasswordGremlinServerContainerFixture() : base("PasswordSecureGremlinServerDockerfile")
            {
            }


            protected override async Task<IGremlinQuerySource> TransformQuerySource(IContainer container, IGremlinQuerySource g) => g
                .UseGremlinServer<Vertex, Edge>(_ => _
                    .At(new UriBuilder("ws", container.Hostname, container.GetMappedPublicPort(8182)).Uri)
                    .ConfigureClientFactory(factory => factory
                        .ConfigureBaseFactory(factory => factory
                            .WithPlainCredentials("stephen", "wrongPassword")))
                    .UseNewtonsoftJson());
        }

        private readonly IGremlinQuerySource _g;

        public WrongPasswordIntegrationTests(WrongPasswordGremlinServerContainerFixture fixture) : base(new JTokenExecutingVerifier())
        {
            _g = fixture.GremlinQuerySource;
        }

        [Fact]
        public async Task Wrong_password_bubbles_up()
        {
            await _g
                .Inject(42)
                .ToArrayAsync()
                .Awaiting(_ => _)
                .Should()
                .ThrowAsync<GremlinQueryExecutionException>()
                .WithInnerException<GremlinQueryExecutionException, ResponseException>()
                .WithMessage("Unauthorized: Username and/or password are incorrect");
        }
    }
}
