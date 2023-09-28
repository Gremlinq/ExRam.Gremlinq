using Boxed.DotnetNewTest;

namespace ExRam.Gremlinq.Templates.Tests
{
    public class RestoreAndBuildTests
    {
        public RestoreAndBuildTests() => DotnetNew.InstallAsync<RestoreAndBuildTests>("ExRam.Gremlinq.sln").Wait();

        [Fact]
        public Task Gremlinq_Console() => Test("gremlinq-console", "Gremlinq.Console.Template.TestProject");

        [Fact]
        public Task Gremlinq_AspNet() => Test("gremlinq-aspnet", "Gremlinq.AspNet.Template.TestProject");

        private async Task Test(string template, string name)
        {
            await using (var tempDirectory = TempDirectory.NewTempDirectory())
            {
                var project = await tempDirectory.DotnetNewAsync(template, name, new Dictionary<string, string> { { "provider", "GremlinServer" } });

                await project.DotnetRestoreAsync();
                await project.DotnetBuildAsync();
            }
        }
    }
}
