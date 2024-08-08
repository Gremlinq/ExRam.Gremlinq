using DotNet.Testcontainers.Images;

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
}
