﻿using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ExRam.Gremlinq.Tests.Fixtures;
using ExRam.Gremlinq.Tests.Infrastructure;

namespace ExRam.Gremlinq.Providers.JanusGraph.Tests
{
    [IntegrationTest("Linux")]
    [IntegrationTest("Windows")]
    public class IntegrationTests : QueryExecutionTest, IClassFixture<JanusGraphContainerFixture>
    {
        public new class Verifier : ExecutingVerifier
        {
            private static readonly Regex RelationIdRegex = new("\"relationId\":[\\s]?\"[0-9a-z]{3}([-][0-9a-z]{3})*\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            public Verifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
            {
            }

            protected override SettingsTask ModifySettingsTask(SettingsTask task) => base
                .ModifySettingsTask(task)
                .ScrubLinesContaining("Traverser>")
                .ScrubRegex(RelationIdRegex, "\"relationId\": \"scrubbed\"");
        }

        public IntegrationTests(JanusGraphContainerFixture fixture) : base(
            fixture,
            new Verifier())
        {
        }
    }
}
