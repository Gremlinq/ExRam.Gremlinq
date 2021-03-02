using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryIntegrationTest : QueryExecutionTest
    {
        private static readonly Regex IdRegex1 = new ("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:Int64\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);
        private static readonly Regex IdRegex2 = new ("\"[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12}([|]PartitionKey)?\"", RegexOptions.IgnoreCase);

        protected QueryIntegrationTest(IGremlinQuerySource g, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(g, testOutputHelper, callerFilePath)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => IdRegex1.Replace(x, "$1\"scrubbed id\"$3"))
                .Add(x => IdRegex2.Replace(x, "\"scrubbed id\""));
        }
    }
}
