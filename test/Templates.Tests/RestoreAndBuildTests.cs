using Boxed.DotnetNewTest;

using TempDirectory = Boxed.DotnetNewTest.TempDirectory;

namespace ExRam.Gremlinq.Templates.Tests
{
    public class RestoreAndBuildTests
    {
        public RestoreAndBuildTests() => DotnetNew.InstallAsync<RestoreAndBuildTests>("ExRam.Gremlinq.slnx").Wait();

        [Theory]
        [InlineData("Neptune", false)]
        [InlineData("CosmosDb", false)]
        [InlineData("JanusGraph", false)]
        [InlineData("GremlinServer", true)]
        [InlineData("GremlinServer", false)]
        public Task Gremlinq_Console(string provider, bool useTestContainers = false) => Test("gremlinq-console", "ConsoleTest", provider, useTestContainers);

        [Theory]
        [InlineData("Neptune", false)]
        [InlineData("CosmosDb", false)]
        [InlineData("JanusGraph", false)]
        [InlineData("GremlinServer", true)]
        [InlineData("GremlinServer", false)]
        public Task Gremlinq_AspNet(string provider, bool useTestContainers = false) => Test("gremlinq-aspnet", "AspNetTest", provider, useTestContainers);

        private async Task Test(string template, string name, string provider, bool useTestContainers)
        {
            await using (var tempDirectory = TempDirectory.NewTempDirectory())
            {
                var project = await tempDirectory.DotnetNewAsync(template, name, new Dictionary<string, string> { { nameof(provider), provider }, { nameof(useTestContainers), useTestContainers.ToString() }, { "version", "13.4.1" } });

                await Task.Delay(500);
                await project.DotnetRestoreAsync();
                await project.DotnetBuildAsync();
            }
        }
    }
}
