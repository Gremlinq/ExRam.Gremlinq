using System.Globalization;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.TestContainers;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    /// <summary>
    /// The facts of <see cref="QueryExecutionTestsBase"/> against a Gremlin Server with string ids. The executor ignores the results,
    /// so every fact proves that its query executes and verifies against the one snapshot <c>Executes</c>, which is <c>[]</c>. A fact
    /// whose query fails keeps its exception as a snapshot of its own.
    /// </summary>
    [IntegrationTest("Linux", true)]
    [IntegrationTest("Windows")]
    public sealed class StringIdIntegrationTests : QueryExecutionTestsBase, IClassFixture<StringIdIntegrationTests.Fixture>
    {
        // The gremlin-server-mod image answers with string ids.
        public sealed class Fixture : GremlinServerContainerFixture
        {
            protected override IGremlinServerConfigurator ConfigureProvider(IGremlinServerConfigurator configurator) => configurator
                .UseGremlinServerModContainer("3.7.7");

            protected override IGremlinQuerySource TransformQuerySource(IGremlinQuerySource g) => base
                .TransformQuerySource(g)
                .ConfigureEnvironment(environment => environment
                    .ConfigureExecutor(_ => _
                        .IgnoreResults()));
        }

        public StringIdIntegrationTests(Fixture fixture) : base(
            fixture,
            new ExecutingVerifier().WithSettings(static task => task.UseSingleSnapshot("Executes")))
        {
        }

        protected override object GetId(long n) => n.ToString(CultureInfo.InvariantCulture);

        // These queries fail by design: fail() fails, and 3.7's choose() fails on an unproductive traverser.
        public override SettingsTask Choose_unproductive() => base.Choose_unproductive().UseMethodName(nameof(Choose_unproductive));

        public override SettingsTask Fail_with_message() => base.Fail_with_message().UseMethodName(nameof(Fail_with_message));

        public override SettingsTask Fail_without_message() => base.Fail_without_message().UseMethodName(nameof(Fail_without_message));

        // These queries compare an id with a number, which a string id never equals, and the server rejects the comparison.
        public override SettingsTask Where_converted_Id_equals_constant() => base.Where_converted_Id_equals_constant().UseMethodName(nameof(Where_converted_Id_equals_constant));

        public override SettingsTask Where_Values_Id_Where() => base.Where_Values_Id_Where().UseMethodName(nameof(Where_Values_Id_Where));
    }
}
