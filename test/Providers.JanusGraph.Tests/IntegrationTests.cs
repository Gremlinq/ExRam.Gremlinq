using System.Text.RegularExpressions;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> against JanusGraph. The executor ignores the results, so every fact proves
    /// that its query executes and verifies against the one snapshot <c>Executes</c>, which is <c>[]</c>. A fact whose query fails
    /// keeps its exception as a snapshot of its own.
    /// </summary>
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public sealed class IntegrationTests : QueryExecutionTestsBase, IClassFixture<JanusGraphContainerFixture>
    {
        private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public IntegrationTests(JanusGraphContainerFixture fixture) : base(
            fixture,
            new ExecutingVerifier().WithSettings(static task => task
                .UseSingleSnapshot("Executes")
                .ScrubLinesContaining("Traverser>")
                .ScrubRegex(RelationIdRegex, "\"relationId\": \"scrubbed\"")))
        {
        }

        // These queries fail by design: fail() fails, and 3.7's choose() fails on an unproductive traverser.
        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseMethodName(nameof(Choose_unproductive));

        public override SettingsTask Fail_with_message() => base.Fail_with_message().UseMethodName(nameof(Fail_with_message));

        public override SettingsTask Fail_without_message() => base.Fail_without_message().UseMethodName(nameof(Fail_without_message));

        // These queries name an element by an id JanusGraph does not give out, or by a string where it expects its own ids.
        public override SettingsTask Properties_Where_Id() => base.Properties_Where_Id().UseMethodName(nameof(Properties_Where_Id));

        public override SettingsTask Properties_Where_Id_equals_static_field() => base.Properties_Where_Id_equals_static_field().UseMethodName(nameof(Properties_Where_Id_equals_static_field));

        public override SettingsTask StringKey() => base.StringKey().UseMethodName(nameof(StringKey));

        public override SettingsTask Where_Id_equals_toStringed_Guid() => base.Where_Id_equals_toStringed_Guid().UseMethodName(nameof(Where_Id_equals_toStringed_Guid));

        public override SettingsTask Where_VertexProperty_Id() => base.Where_VertexProperty_Id().UseMethodName(nameof(Where_VertexProperty_Id));
    }
}
