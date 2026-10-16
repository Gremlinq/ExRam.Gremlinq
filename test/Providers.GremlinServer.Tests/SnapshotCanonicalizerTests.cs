using ExRam.Gremlinq.Tests.Infrastructure;

using FluentAssertions;

namespace ExRam.Gremlinq.Providers.GremlinServer.Tests
{
    public class SnapshotCanonicalizerTests
    {
        private static readonly string SnapshotDirectory = Path.GetDirectoryName(SourceFileName.Of<IntegrationWithoutProjectionTests>())!;

        [Fact]
        public void Every_snapshot_of_IntegrationWithoutProjectionTests_parses()
        {
            var paths = Directory.GetFiles(SnapshotDirectory, $"{nameof(IntegrationWithoutProjectionTests)}.*.verified.txt");

            paths.Should().NotBeEmpty();

            foreach (var path in paths)
            {
                File.ReadAllText(path)
                    .Invoking(SnapshotParser.Parse)
                    .Should()
                    .NotThrow(path);
            }
        }

        [Fact]
        public void An_exception_document_becomes_EXCEPTION()
        {
            var node = SnapshotParser.Parse(ReadSnapshot("Fail_with_message"));

            node.Should().Be(new SnapshotScalar(SnapshotParser.Exception));
            SnapshotCanonicalizer.Canonicalize(node).Should().Be("EXCEPTION");
        }

        [Fact]
        public void Group_keeps_its_repeated_keys_and_sorts_its_members()
        {
            var node = SnapshotParser.Parse(ReadSnapshot("Group"));

            var group = node.Should().BeOfType<SnapshotArray>().Subject.Items.Should().ContainSingle().Subject.Should().BeOfType<SnapshotObject>().Subject;

            group.Members.Should().HaveCountGreaterThan(1);
            group.Members.Select(static member => member.Key).Distinct().Should().ContainSingle("Verify writes a list of key/value pairs as an object with repeated keys");

            var memberLines = SnapshotCanonicalizer.Canonicalize(node)
                .Split('\n')
                .Where(static line => line.StartsWith("    ExRam.", StringComparison.Ordinal))
                .ToArray();

            memberLines.Should().HaveCount(group.Members.Count);
            memberLines.Should().BeInAscendingOrder(StringComparer.Ordinal);
        }

        [Fact]
        public void Tree_with_builder_nests_key_value_arrays()
        {
            var node = SnapshotParser.Parse(ReadSnapshot("Tree_with_builder"));

            var tree = node.Should().BeOfType<SnapshotArray>().Subject.Items.Should().ContainSingle().Subject.Should().BeOfType<SnapshotArray>().Subject;

            tree.Items.Should().NotBeEmpty();

            foreach (var entry in tree.Items)
            {
                var members = entry.Should().BeOfType<SnapshotObject>().Subject.Members;

                members.Select(static member => member.Key).Should().Equal("Key", "Value");
                members[0].Value.Should().BeOfType<SnapshotObject>().Which.Members.Should().Contain(new KeyValuePair<string, SnapshotNode>("Label", new SnapshotScalar("RichVertex")));
                members[1].Value.Should().BeOfType<SnapshotArray>();
            }

            SnapshotCanonicalizer.Canonicalize(node).Should().Contain("Value: []");
        }

        [Fact]
        public void Path_has_labels_and_objects()
        {
            var paths = SnapshotParser.Parse(ReadSnapshot("Path")).Should().BeOfType<SnapshotArray>().Subject.Items;

            paths.Should().HaveCount(2);

            foreach (var path in paths)
            {
                var members = path.Should().BeOfType<SnapshotObject>().Subject.Members;

                members.Select(static member => member.Key).Should().Equal("Labels", "Objects");
                members[0].Value.Should().BeOfType<SnapshotArray>().Which.Items.Should().HaveCount(3).And.AllBeEquivalentTo(new SnapshotArray([]));
                members[1].Value.Should().BeOfType<SnapshotArray>().Which.Items.Should().HaveCount(3).And.AllBeOfType<SnapshotObject>();
            }
        }

        [Fact]
        public void The_canonical_form_ignores_order_but_not_multiplicity()
        {
            const string first = """
                [
                  {
                    Id: 1,
                    Label: A
                  },
                  7,
                  7
                ]
                """;

            const string second = """
                [
                  7,
                  {
                    Label: A,
                    Id: 1
                  },
                  7
                ]
                """;

            const string third = """
                [
                  7,
                  {
                    Label: A,
                    Id: 1
                  }
                ]
                """;

            SnapshotCanonicalizer.Canonicalize(first).Should().Be(SnapshotCanonicalizer.Canonicalize(second));
            SnapshotCanonicalizer.Canonicalize(first).Should().NotBe(SnapshotCanonicalizer.Canonicalize(third));
        }

        [Fact]
        public void The_canonical_form_has_one_node_per_line()
        {
            const string snapshot = """
                {
                  B: [
                    2,
                    1
                  ],
                  A: {},
                  C: []
                }
                """;

            SnapshotCanonicalizer.Canonicalize(snapshot).Should().Be("{\n  A: {}\n  B: [\n    1\n    2\n  ]\n  C: []\n}");
        }

        [Fact]
        public void Empty_containers_and_empty_strings_parse()
        {
            var node = SnapshotParser.Parse("""
                [
                  {
                    Properties: {},
                    Values: [],
                    Value: ,
                    Date: 2020-04-07 14:43:36 +0
                  }
                ]
                """);

            var members = node.Should().BeOfType<SnapshotArray>().Subject.Items.Should().ContainSingle().Subject.Should().BeOfType<SnapshotObject>().Subject.Members;

            // Record equality compares the lists by reference, so the comparison is structural.
            members.Should().BeEquivalentTo(
                [
                    new KeyValuePair<string, SnapshotNode>("Properties", new SnapshotObject([])),
                    new KeyValuePair<string, SnapshotNode>("Values", new SnapshotArray([])),
                    new KeyValuePair<string, SnapshotNode>("Value", new SnapshotScalar(string.Empty)),
                    new KeyValuePair<string, SnapshotNode>("Date", new SnapshotScalar("2020-04-07 14:43:36 +0"))
                ],
                static options => options.WithStrictOrdering().RespectingRuntimeTypes());
        }

        [Fact]
        public void An_empty_string_as_the_last_member_parses() => SnapshotParser
            .Parse("{\n  A: 1,\n  B: \n}")
            .Should()
            .BeEquivalentTo(
                new SnapshotObject(
                [
                    new KeyValuePair<string, SnapshotNode>("A", new SnapshotScalar("1")),
                    new KeyValuePair<string, SnapshotNode>("B", new SnapshotScalar(string.Empty))
                ]),
                static options => options.WithStrictOrdering().RespectingRuntimeTypes());

        [Fact]
        public void An_empty_document_is_an_empty_scalar() => SnapshotParser
            .Parse("\n")
            .Should()
            .Be(new SnapshotScalar(string.Empty));

        [Theory]
        [InlineData("[\n  1,\n")]
        [InlineData("{\n  A: 1,\n")]
        [InlineData("{\n  no member\n}")]
        [InlineData("[\n  1\n]\n2")]
        public void Broken_text_throws_a_FormatException(string text) => text
            .Invoking(SnapshotParser.Parse)
            .Should()
            .Throw<FormatException>();

        private static string ReadSnapshot(string name) => File.ReadAllText(Path.Combine(SnapshotDirectory, $"{nameof(IntegrationWithoutProjectionTests)}.{name}.verified.txt"));
    }
}
