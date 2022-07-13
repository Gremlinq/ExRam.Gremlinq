﻿using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class QueryIntegrationTest : QueryExecutionTest
    {
        public abstract class Fixture : GremlinqTestFixture
        {
            protected Fixture(IGremlinQuerySource source) : base(source
                .ConfigureEnvironment(env => env
                    .UseDeserializer(GremlinQueryExecutionResultDeserializer.Default)))
            {
            }
        }

        private static readonly Regex GuidRegex = new("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);
        private static readonly Regex IdRegex = new ("(\"id\"\\s*[:,]\\s*{\\s*\"@type\"\\s*:\\s*\"g:(Int32|Int64|UUID)\"\\s*,\\s*\"@value\":\\s*)([^\\s{}]+)(\\s*})", RegexOptions.IgnoreCase);

        protected QueryIntegrationTest(Fixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(
            fixture,
            testOutputHelper,
            callerFilePath)
        {
        }

        public override IImmutableList<Func<string, string>> Scrubbers()
        {
            return base.Scrubbers()
                .Add(x => IdRegex.Replace(x, "$1-1$4"))
                .Add(x => GuidRegex.Replace(x, "$1\"scrubbed uuid\"$4"));
        }

        [Fact]
        public async Task Dynamic()
        {
            var first = await _g
                .V<Person>()
                .Cast<dynamic>()
                .FirstAsync();

            await Verify(first.Name);
        }
    }
}
