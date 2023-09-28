using Boxed.DotnetNewTest;

namespace ExRam.Gremlinq.Templates.Tests
{
    public class RestoreAndBuildTests
    {
        public RestoreAndBuildTests() => DotnetNew.InstallAsync<RestoreAndBuildTests>("ExRam.Gremlinq.sln").Wait();

        [Fact]
        public async Task Gremlinq_Console()
        {
            await using (var tempDirectory = TempDirectory.NewTempDirectory())
            {
                var project = await tempDirectory.DotnetNewAsync("gremlinq-console", "Gremlinq.Console.Template.TestProject");

                await project.DotnetRestoreAsync();
                await project.DotnetBuildAsync();
            }
        }

        [Fact]
        public async Task Gremlinq_AspNet()
        {
            await using (var tempDirectory = TempDirectory.NewTempDirectory())
            {
                var project = await tempDirectory.DotnetNewAsync("gremlinq-aspnet", "Gremlinq.AspNet.Template.TestProject");

                await project.DotnetRestoreAsync();
                await project.DotnetBuildAsync();
            }
        }
    }
}
