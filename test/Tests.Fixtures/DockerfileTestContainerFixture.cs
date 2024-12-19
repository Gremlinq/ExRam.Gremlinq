using System.Runtime.CompilerServices;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public abstract class DockerfileTestContainerFixture : TestContainerFixtureBase
    {
        private readonly string _dockerfile;
        private readonly string _callerFilePath;

        protected DockerfileTestContainerFixture(string dockerfile, int port = 8182) : this(dockerfile, port, 0)
        {
        }

        private DockerfileTestContainerFixture(string dockerfile, int port = 8182, int _ = 0, [CallerFilePath] string callerFilePath = "") : base(port)
        {
            _dockerfile = dockerfile;
            _callerFilePath = callerFilePath;
        }

        protected override async Task<IImage> GetImage()
        {
            var futureImage = new ImageFromDockerfileBuilder()
                .WithDockerfileDirectory(Path.Combine(Path.GetDirectoryName(_callerFilePath)!, "Dockerfiles"))
                .WithDockerfile(_dockerfile)
                .Build();

            await futureImage.CreateAsync();

            return futureImage;
        }
    }
}
