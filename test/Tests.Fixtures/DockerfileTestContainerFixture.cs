using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Tests.Fixtures
{
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
                .WithDockerfileDirectory(Path.Combine(Path.GetDirectoryName(SourceFileName.OfThis())!, "Dockerfiles"))
                .WithDockerfile(_dockerfile)
                .Build();

            await futureImage.CreateAsync();

            return futureImage;
        }
    }
}
