using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class DockerfileTestContainerFixture : TestContainerFixtureBase
    {
        private readonly string _dockerfile;
        private readonly IReadOnlyDictionary<string, string> _buildArguments;

        protected DockerfileTestContainerFixture(string dockerfile, int port = 8182, IReadOnlyDictionary<string, string>? buildArguments = null) : base(port)
        {
            _dockerfile = dockerfile;
            _buildArguments = buildArguments ?? new Dictionary<string, string>();
        }

        protected override async Task<IImage> GetImage()
        {
            var builder = new ImageFromDockerfileBuilder()
                .WithDockerfileDirectory(Path.Combine(Path.GetDirectoryName(SourceFileName.OfThis())!, "Dockerfiles"))
                .WithDockerfile(_dockerfile);

            foreach (var (name, value) in _buildArguments)
            {
                builder = builder.WithBuildArgument(name, value);
            }

            var futureImage = builder
                .Build();

            await futureImage.CreateAsync();

            return futureImage;
        }
    }
}
