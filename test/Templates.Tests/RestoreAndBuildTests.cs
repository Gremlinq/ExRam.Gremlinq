using Boxed.DotnetNewTest;

namespace ExRam.Gremlinq.Templates.Tests
{
    public class RestoreAndBuildTests
    {
        public RestoreAndBuildTests() => DotnetNew.InstallAsync<RestoreAndBuildTests>("ExRam.Gremlinq.sln").Wait();

        [Theory]
        [InlineData("GremlinServer")]
        [InlineData("CosmosDb")]
        [InlineData("Neptune")]
        [InlineData("JanusGraph")]
        public Task Gremlinq_Console(string provider) => Test("gremlinq-console", "Gremlinq.Console.Template.TestProject", provider);

        [Theory]
        [InlineData("GremlinServer")]
        [InlineData("CosmosDb")]
        [InlineData("Neptune")]
        [InlineData("JanusGraph")]
        public Task Gremlinq_AspNet(string provider) => Test("gremlinq-aspnet", "Gremlinq.AspNet.Template.TestProject", provider);

        private async Task Test(string template, string name, string provider)
        {
            await using (var tempDirectory = TempDirectory.NewTempDirectory())
            {
                var project = await tempDirectory.DotnetNewAsync(template, name, new Dictionary<string, string> { { "provider", provider }, { "version", "12.0.0-preview.1077" } });

                await project.DotnetRestoreAsync();
                await project.DotnetBuildAsync();
            }
        }
    }
}
