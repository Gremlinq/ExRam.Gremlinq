using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core.Tests;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public abstract class IntegrationTestsBase : QueryExecutionTest
    {
        private static readonly Regex IdRegex = new ("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(Int32|Int64|UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);

        protected IntegrationTestsBase(IntegrationTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers() => ImmutableList<Func<string, string>>.Empty
            .Add(x => IdRegex.Replace(x, "$1-1$4"))
            .ScrubGuids();
    }
}
